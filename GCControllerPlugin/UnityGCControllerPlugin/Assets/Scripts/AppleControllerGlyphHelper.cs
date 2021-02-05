using HovelHouse.GameController;
using UnityEngine;

public class AppleControllerGlyphHelper : AppleControllerGlyphHelperBase
{
    public AppleControllerGlyphHelper(
        IRewiredAppleControllerAdapter adapter,
        SFSymbolSet glyphSet) 
        : base (adapter, glyphSet)
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
