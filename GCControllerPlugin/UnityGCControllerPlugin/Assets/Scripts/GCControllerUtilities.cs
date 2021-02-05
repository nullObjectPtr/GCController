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

public class SFSymbolSet
{
	public enum LoadMode
	{
		Replace,
		Append
	}
	
    private Dictionary<string, Texture2D> _textures;
    private Dictionary<string, Sprite> _sprites;
    private UIImageSymbolConfiguration _traits;
    private SymbolInfoCollection _symbolInfoCollection;
    
    public SFSymbolSet()
    {
        _textures = new Dictionary<string, Texture2D>();
        _sprites = new Dictionary<string, Sprite>();
    }
    
    public SFSymbolSet(
        double pointSize, UIImageSymbolWeight weight, UIImageSymbolScale scale
    )
    {
        _textures = new Dictionary<string, Texture2D>();
        _sprites = new Dictionary<string, Sprite>();
        _traits = UIImageSymbolConfiguration.ConfigurationWithPointSize(pointSize, weight, scale);
    }
    
    public SFSymbolSet(
        double pointSize,
        UIImageSymbolWeight weight = UIImageSymbolWeight.Unspecified
        )
    {
        _textures = new Dictionary<string, Texture2D>();
        _sprites = new Dictionary<string, Sprite>();
        _traits = UIImageSymbolConfiguration.ConfigurationWithPointSize(pointSize, weight);
    }

    public Sprite getSprite(string id, bool filled)
    {
        if(string.IsNullOrEmpty(id))
            throw new ArgumentNullException(nameof(id));

        if (filled
            && SymbolInfoCollection.AllControllerSymbols.TryGetFilledVariant(id, out var filledId))
        {
            id = filledId;
        }

        if (_sprites.TryGetValue(id, out var sprite))
        {
            Debug.Log($"Got sprite {id}");
            return sprite;
        }

        Debug.LogError($"could not find sprite with id '{id}'");
        return null;
    }
    
    public Texture2D getTexture(string id, bool filled)
    {
        if(string.IsNullOrEmpty(id))
            throw new ArgumentNullException(nameof(id));
        
        if (filled
            && SymbolInfoCollection.AllControllerSymbols.TryGetFilledVariant(id, out var filledId))
        {
            id = filledId;
        }

        if (_textures.TryGetValue(id, out var texture))
            return texture;

        Debug.LogError($"could not find texture with id '{id}'");
        return null;
    }

    private void LoadTexturesInternal(IEnumerable<string> textureIds, LoadMode mode)
    {
        foreach (var id in textureIds)
        {
            // How are these getting in here anyway?
            if (string.IsNullOrEmpty(id))
                continue;

            // In append mode - skip over any images that are already loaded
            if (mode == LoadMode.Append && _textures[id] != null)
				continue;
            
            var image = UIImage.SystemImageNamed(id, _traits);
            if (image == null)
            {
                Debug.LogWarning($"no glyph found for name '{id}'");
                continue;
            }
            
            Debug.Log($"loaded image for id: {id}");

            var texture = new Texture2D(1, 1);
            texture.LoadImage(UIImage.PNGRepresentation(image));

            _textures[id] = texture;
            _sprites[id]  = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height), 
                new Vector2(0.5f, 0.5f));
        }
    }

    /// <summary>
    /// Loads all symbols for a given controller's physical input profile. Use this if you can't be
    /// sure that you've preloaded all the possible glyphs
    /// </summary>
    /// <param name="controller"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void LoadTexturesForController(GCController controller, LoadMode mode = LoadMode.Append)
    {
        if(controller == null)
            throw new ArgumentNullException(nameof(controller));
    
        // Code smell - refactor this line
        _symbolInfoCollection = SymbolInfoCollection.AllControllerSymbols;
        
        Debug.Log($"Loading textures for controller {controller.VendorName}");
    
        var textureNames = controller.PhysicalInputProfile.Elements
            .SelectMany(x => { return new[] {x.Item2.SfSymbolsName, x.Item2.UnmappedSfSymbolsName}; })
            .Where(symbolName => string.IsNullOrEmpty(symbolName) == false)
            .Distinct();
        
        LoadTexturesInternal(textureNames, mode);
    }

    /// <summary>
    /// Loads all SFSymbols in the "gaming" category
    /// </summary>
    /// <param name="loadFills"></param>
    public void LoadAllControllerGlyphs(bool loadFills, LoadMode loadMode = LoadMode.Replace)
    {
        // Code smell - refactor this line
        _symbolInfoCollection = SymbolInfoCollection.AllControllerSymbols;
        
        LoadTexturesInternal(_symbolInfoCollection.Symbols.Select(g => g.symbolName), loadMode);
        if (!loadFills) return;
        
        var fillSymbolNames = SymbolInfoCollection.AllControllerSymbols.Symbols
            .Where(g => g.hasFill)
            .Select(g => g.filledSymbolName);
        LoadTexturesInternal(fillSymbolNames, loadMode);
    }
}
