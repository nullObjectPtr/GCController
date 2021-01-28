using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rewired;
using UnityEngine;

public class FallbackGlyphHelper : IGlyphHelper
{
    private ControllerElementToSpriteMap _fallbackMap;
    private List<ActionElementMap> results = new List<ActionElementMap>();
    
    public FallbackGlyphHelper(ControllerElementToSpriteMap fallbackMap)
    {
        _fallbackMap = fallbackMap ?? throw new ArgumentNullException(nameof(fallbackMap));
    }
    
    public Sprite GetFirstGlyphForRewiredAction(Player player, string actionName)
    {
        var action = ReInput.mapping.GetAction(actionName);
        if(action == null)
            throw new ArgumentException($"no such rewired action with name: '{actionName}'");

        return GetGlyphsForRewiredAction(player, action).FirstOrDefault();
    }

    public Sprite GetFirstGlyphForRewiredAction(Player player, int actionId)
    {
        var action = ReInput.mapping.GetAction(actionId);
        if(action == null)
            throw new ArgumentException($"no such rewired action with id: '{actionId}'");

        return GetGlyphsForRewiredAction(player, action).FirstOrDefault();
    }

    public IEnumerable<Sprite> GetAllGlyphsForRewiredAction(Player player, int actionId)
    {
        var action = ReInput.mapping.GetAction(actionId);
        if(action == null)
            throw new ArgumentException($"no such rewired action with id: '{actionId}'");

        return GetGlyphsForRewiredAction(player, action);
    }

    public IEnumerable<Sprite> GetAllGlyphsForRewiredAction(Player player, string actionName)
    {
        var action = ReInput.mapping.GetAction(actionName);
        if(action == null)
            throw new ArgumentException($"no such rewired action with name: '{actionName}'");

        return GetGlyphsForRewiredAction(player, action);
    }

    public IEnumerable<Sprite> GetGlyphsForRewiredAction(Player player, InputAction action)
    {
        if(player == null)
            throw new ArgumentNullException(nameof(player));
        
        if(action == null)
            throw new ArgumentNullException(nameof(action));

        Debug.Log($"GetGlyphForRewiredAction: {action.name}");

        // Get the last active controller, if player has not connected a controller
        // yet determine a fallback
        var controller = player.controllers.GetLastActiveController() ?? (player.controllers.joystickCount > 0
            ? player.controllers.Joysticks[0]
            : (Controller) player.controllers.Keyboard);

        var actionElementMap = player.controllers.maps.GetFirstElementMapWithAction(
            controller, action.id, true);
        
        var count = player.controllers.maps.GetElementMapsWithAction(
            controller, action.id, true, results);
        
        // Nothing bound to this
        if (count == 0)
            return null;

        return results.Select(result =>
        {
            var elem = controller.GetElementById(actionElementMap.elementIdentifierId);
            return GetSymbolForRewiredElementId(elem);
        });
    }

    Sprite GetSymbolForRewiredElementId(Controller.Element element)
    {
        if(element == null)
            throw new ArgumentNullException(nameof(element));
        
        Debug.Log($"GetSymbolForElement: {element.elementIdentifier.name}");

        return _fallbackMap.GetSpriteForElement(element.elementIdentifier.name);
    }
}
