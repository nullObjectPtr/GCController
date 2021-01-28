using System;
using System.Collections.Generic;
using System.Linq;
using HovelHouse.GameController;
using Rewired;
using UnityEngine;

public class AppleControllerGlyphHelper : IGlyphHelper
{
    private SFSymbolSet _glyphSet;
    private IRewiredAppleControllerAdapter _adapter;
    private List<ActionElementMap> results = new List<ActionElementMap>();
    
    public AppleControllerGlyphHelper(
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

        return _adapter.GetGCElementForRewiredElementId(
            actionElementMap.elementType, actionElementMap.elementIdentifierId);
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
            var ctrlr = mpe.controllerMap.controller;
            Debug.Log($"ctrl:{ctrlr.name}({ctrlr.identifier.deviceInstanceGuid}) - \nmap:'{mpe.controllerMap.name}'\n elem:{mpe.elementIdentifierId}");
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
        
        var element = adapter.GetGCElementForRewiredElementId(
            actionElementMap.elementType, actionElementMap.elementIdentifierId);

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

    private string GetSymbolNameForElement(GCControllerElement element)
    {
        var symbolName = element.SfSymbolsName;
        var unmappedSymbolName = element.UnmappedSfSymbolsName;

        // don't ask me why, but sometimes the SfSymbolName is null
        // but the un-mapped symbol is not
        // so we need to do a fallback thing here
        var symbol = symbolName ?? unmappedSymbolName;

        if (string.IsNullOrEmpty(symbol) == false)
            return symbol;
        
        var parentSymbol = element.Collection != null ? (element.Collection.SfSymbolsName ?? element.Collection.UnmappedSfSymbolsName) : "";
        Debug.Log($"parent symbol '{parentSymbol}'");

        if (parentSymbol != null)
            return parentSymbol;
        
        // Workaround for a bug where BigSur does not return the d-pad
        // symbols for the d-pad elements, 
        // this is future-proofed a bit, if they ever patch this later
        // this block of code won't be hit

        var extendedGamepad = _adapter.AppleController.ExtendedGamepad;
        if (extendedGamepad != null)
        {
            if (element == extendedGamepad.Dpad.Down)
            {
                return "dpad.down.fill";
            }

            if (element == extendedGamepad.Dpad.Left)
            {
                return "dpad.left.fill";
            }

            if (element == extendedGamepad.Dpad.Right)
            {
                return "dpad.right.fill";
            }

            if (element == extendedGamepad.Dpad.Up)
            {
                return "dpad.up.fill";
            }
            
            if (element == extendedGamepad.LeftThumbstick.XAxis || element == extendedGamepad.LeftThumbstick.YAxis)
            {
                return "l.joystick";
            }

            if (element == extendedGamepad.RightThumbstick.XAxis || element == extendedGamepad.RightThumbstick.YAxis)
            {
                return "r.joystick";
            }

            Debug.LogWarning("no sf symbol is available for element");
            return null;
        }

        var microGamepad = _adapter.AppleController.MicroGamepad;
        if (microGamepad != null)
        {
            if (element == microGamepad.ButtonA)
            {
                return "a.circle";
            }

            if (element == microGamepad.ButtonX)
            {
                return "x.circle";
            }

            if (element == microGamepad.ButtonMenu)
            {
                return "line.horizontal.3.circle";
            }

            if (element == microGamepad.Dpad.XAxis || element == microGamepad.Dpad.YAxis)
            {
                return "dpad";
            }
        }

        Debug.LogWarning("no sf symbol is available for element");
        return null;
    }
}
