using System;
using System.Collections.Generic;
using System.Linq;
using HovelHouse.GameController;
using Rewired;
using UnityEngine;

public abstract class AppleControllerGlyphHelperBase : IGlyphHelper
{
    protected readonly IRewiredAppleControllerAdapter _adapter;

    private readonly SFSymbolSet _glyphSet;
    private readonly List<ActionElementMap> results = new List<ActionElementMap>();
    
    public AppleControllerGlyphHelperBase(
        IRewiredAppleControllerAdapter adapter,
        SFSymbolSet glyphSet)
    {
        _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        _glyphSet = glyphSet ?? throw new ArgumentNullException(nameof(glyphSet));
    }
    
    public Sprite GetFirstGlyphForRewiredAction(Player player, string actionName)
    {
        var action = ReInput.mapping.GetAction(actionName);
        if(action == null)
            throw new ArgumentException($"no such rewired action with name: '{actionName}'");

        return GetGlyphForRewiredAction(player, action).FirstOrDefault();
    }

    public Sprite GetFirstGlyphForRewiredAction(Player player, int actionId)
    {
        var action = ReInput.mapping.GetAction(actionId);
        if(action == null)
            throw new ArgumentException($"no such rewired action with id: '{actionId}'");

        return GetGlyphForRewiredAction(player, action).FirstOrDefault();
    }

    public IEnumerable<Sprite> GetAllGlyphsForRewiredAction(Player player, int actionId)
    {
        var action = ReInput.mapping.GetAction(actionId);
        if(action == null)
            throw new ArgumentException($"no such rewired action with id: '{actionId}'");

        return GetGlyphForRewiredAction(player, action);
    }

    public IEnumerable<Sprite> GetAllGlyphsForRewiredAction(Player player, string actionName)
    {
        var action = ReInput.mapping.GetAction(actionName);
        if(action == null)
            throw new ArgumentException($"no such rewired action with name: '{actionName}'");
        
        return GetGlyphForRewiredAction(player, action);
    }
    
    public GCControllerElement GetControllerElementForRewiredAction(Player player, int actionId)
    {
        var action = ReInput.mapping.GetAction(actionId);
        
        var actionElementMap = GetActionElementMap(player, action).
            FirstOrDefault();

        if (actionElementMap == null)
        {
            Debug.LogError($"no element bound to action {action.name}");
        }

        return _adapter.GetGCElementForRewiredElementName(actionElementMap.elementIdentifierName);
    }

    private IEnumerable<Sprite> GetGlyphForRewiredAction(Player player, InputAction action)
    {
        if(player == null)
            throw new ArgumentNullException(nameof(player));
        
        if(action == null)
            throw new ArgumentNullException(nameof(action));
        
        Debug.Log($"Get glyph for rewired action: {action.name}");

        var maps = GetActionElementMap(player, action);

        return maps.Select(aem =>
        {
            return GetSymbolForRewiredElementId(aem, _adapter);
        });
    }

    private List<ActionElementMap> GetActionElementMap(Player player, InputAction action)
    {
        // Why isn't this working with the x-box elite controller?
        var count = player.controllers.maps.GetElementMapsWithAction(
            ControllerType.Custom, action.id, true, results);

        // var count = player.controllers.maps.GetElementMapsWithAction(action.id, true, results);

        foreach (var mpe in results)
        {
            var controller = mpe.controllerMap.controller;
            Debug.Log($"ctrl:{controller.name}({controller.identifier.deviceInstanceGuid}) - \nmap:'{mpe.controllerMap.name}'\n elem:{mpe.elementIdentifierId}");
        }
        
        // Nothing bound to this
        if (count == 0)
        {
            Debug.LogError($"nothing bound to action {action.name}");
            return new List<ActionElementMap>();
        }

        return results;
    }

    private Sprite GetSymbolForRewiredElementId(
        ActionElementMap actionElementMap, IRewiredAppleControllerAdapter adapter)
    {
        // maps a rewired element to an apple element so we can access the
        // symbol name property, which changes at run-time when the user remaps their
        // controller hardware in settings
        
        var element = adapter.GetGCElementForRewiredElementName(
            actionElementMap.elementIdentifierName);

        // Nothing bound to this - did someone change the controller definition?
        if (element == null)
        {
            Debug.LogWarning("GCExtendedGamepad does not contain an element of type {type} mapped to id {id}");
            return null;
        }

        var symbolName = GetSymbolNameForElement(element);
        
        return string.IsNullOrEmpty(symbolName) 
            ? null 
            : _glyphSet.getSprite(symbolName);
    }

    // for a given controller element, return the sfsymbol name for it
    protected abstract string GetSymbolNameForElement(GCControllerElement element);
}
