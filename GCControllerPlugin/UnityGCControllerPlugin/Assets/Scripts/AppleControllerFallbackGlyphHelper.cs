﻿using System.Linq;
using HovelHouse.GameController;

public class AppleControllerFallbackGlyphHelper : AppleControllerGlyphHelperBase
{
    private GCControllerElementToSFSymbolNameMap _symbolNameMap;
    public AppleControllerFallbackGlyphHelper(
        IRewiredAppleControllerAdapter adapter, IGlyphProvider glyphProvider, GCControllerElementToSFSymbolNameMap fallbackMap)
        : base(adapter, glyphProvider)
    {
        _symbolNameMap = fallbackMap;
    }
    
    protected override string GetSymbolNameForElement(GCControllerElement element)
    {
        var elementName = _adapter.GetElementName(element);
        var entry = _symbolNameMap.Records.FirstOrDefault(r => r.elementType == elementName);
        return entry?.sfSymbolName;
    }
}