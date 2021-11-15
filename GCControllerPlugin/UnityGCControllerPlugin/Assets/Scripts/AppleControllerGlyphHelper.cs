using HovelHouse.GameController;
using UnityEngine;

public class AppleControllerGlyphHelper : AppleControllerGlyphHelperBase
{
    public AppleControllerGlyphHelper(
        IRewiredAppleControllerAdapter adapter,
        IGlyphProvider glyphProvider) 
        : base (adapter, glyphProvider)
    {
    }

    protected override string GetSymbolNameForElement(GCControllerElement element)
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

        if (string.IsNullOrEmpty(parentSymbol) == false)
            return parentSymbol;
        
        Debug.LogWarning($"OS reports no symbol available for element {element.LocalizedName}. Attempting fallback behavior");
        
        // Workaround for a bug where the API does not return
        // symbols for the d-pad elements, 
        // this is future-proofed a bit, if they ever patch this later
        // this block of code won't be hit

        var extendedGamepad = _adapter.AppleController.ExtendedGamepad;
        var microGamepad = _adapter.AppleController.MicroGamepad;

        if (microGamepad != null)
        {
            if (element == microGamepad.Dpad || element == microGamepad.DpadRing || element == microGamepad.Dpad.XAxis || element == microGamepad.Dpad.YAxis)
            {
                return "dpad";
            }

            if (element == microGamepad.Dpad.Down || element == microGamepad.DpadRing.Down)
            {
                return "dpad.down.fill";
            }

            if (element == microGamepad.Dpad.Left || element == microGamepad.DpadRing.Left)
            {
                return "dpad.left.fill";
            }

            if (element == microGamepad.Dpad.Right || element == microGamepad.DpadRing.Right)
            {
                return "dpad.right.fill";
            }

            if (element == microGamepad.Dpad.Up || element == microGamepad.DpadRing.Up)
            {
                return "dpad.up.fill";
            }
            
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
        }

        if(extendedGamepad != null)
        {
            if (element == extendedGamepad.LeftThumbstick.XAxis || element == extendedGamepad.LeftThumbstick.YAxis)
            {
                return "l.joystick";
            }

            if (element == extendedGamepad.RightThumbstick.XAxis || element == extendedGamepad.RightThumbstick.YAxis)
            {
                return "r.joystick";
            }
        }

        Debug.LogWarning($"no sf symbol is available for element {element.LocalizedName}");
        return null;
    }
}
