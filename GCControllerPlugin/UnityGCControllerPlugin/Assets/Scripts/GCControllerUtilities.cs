using System;
using System.Collections.Generic;
using System.Linq;
using HovelHouse.GameController;
using UnityEngine;

public struct SFSymbolInfo
{
    public readonly string symbolName;
    public readonly string filledSymbolName;
    public readonly string altFilledSymbolName;

    public bool hasFill => string.IsNullOrEmpty(filledSymbolName) == false;
    public bool hasAltFill => string.IsNullOrEmpty(altFilledSymbolName) == false;
    
    public SFSymbolInfo(string symbolName)
    {
        this.symbolName = symbolName;
        filledSymbolName = null;
        altFilledSymbolName = null;
    }

    public SFSymbolInfo(string symbolName, string filledSymbolName)
    {
        this.symbolName = symbolName;
        this.filledSymbolName = filledSymbolName;
        altFilledSymbolName = null;
    }

    public SFSymbolInfo(string symbolName, string filledSymbolName, string altFilledSymbolName)
    {
	    this.symbolName = symbolName;
	    this.filledSymbolName = filledSymbolName;
	    this.altFilledSymbolName = altFilledSymbolName;
    }
}

/**
 * A version of the glyph provider that loads a controller glyph on-demand and performs
 * no pre-fetching and no caching
 */
public class OnDemandGlyphProvider : IGlyphProvider
{
    private UIImageSymbolConfiguration _traits;

    public OnDemandGlyphProvider(
        double pointSize,
        UIImageSymbolWeight weight = UIImageSymbolWeight.Unspecified
    )
    {
        _traits = UIImageSymbolConfiguration.ConfigurationWithPointSize(pointSize, weight);
    }
    
    public Sprite GetSprite(string name, bool filled)
    {
        UIImage image;
        
        if (filled && SymbolInfoCollection.AllControllerSymbols.TryGetFilledVariant(name, out var filledName))
        {
            image = UIImage.SystemImageNamed(filledName, _traits);
        }
        else
        {
            image = UIImage.SystemImageNamed(name, _traits);
        }
        
        if (image == null)
        {
            Debug.LogWarning($"no glyph found for name '{name}'");
            return null;
        }
            
        //Debug.Log($"loaded image for id: {name}");

        var texture = new Texture2D(1, 1);
        texture.LoadImage(UIImage.PNGRepresentation(image));

        var sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
        
        return sprite;
    }
}
