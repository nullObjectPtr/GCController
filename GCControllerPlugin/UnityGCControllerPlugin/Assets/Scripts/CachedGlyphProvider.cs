using System;
using System.Collections.Generic;
using System.Linq;
using HovelHouse.GameController;
using UnityEngine;

/**
 * A version of the glyph provider that loads all of the controller glyphs at once
 * and caches them
 */
public class CachedGlyphProvider : IGlyphProvider
{
	public enum LoadMode
	{
		Replace,
		Append
	}

    public enum LoadFillOption
    {
        LoadUnfilled,
        LoadFilled,
        LoadBoth
    }
	
    private Dictionary<string, Sprite> _sprites;
    private UIImageSymbolConfiguration _traits;
    private SymbolInfoCollection _symbolInfoCollection;
    
    public CachedGlyphProvider()
    {
        _sprites = new Dictionary<string, Sprite>();
    }
    
    public CachedGlyphProvider(
        double pointSize, UIImageSymbolWeight weight, UIImageSymbolScale scale
    )
    {
        _sprites = new Dictionary<string, Sprite>();
        _traits = UIImageSymbolConfiguration.ConfigurationWithPointSize(pointSize, weight, scale);
    }
    
    public CachedGlyphProvider(
        double pointSize,
        UIImageSymbolWeight weight = UIImageSymbolWeight.Unspecified
        )
    {
        _sprites = new Dictionary<string, Sprite>();
        _traits = UIImageSymbolConfiguration.ConfigurationWithPointSize(pointSize, weight);
    }

    public Sprite GetSprite(string id, bool filled)
    {
        if(string.IsNullOrEmpty(id))
            throw new ArgumentNullException(nameof(id));

        if (filled
            && SymbolInfoCollection.AllControllerSymbols.TryGetFilledVariant(id, out var filledId))
        {
            id = filledId;
        }

        if (_sprites.TryGetValue(id, out _) == false)
        {
            LoadTexture(id, LoadMode.Append);
        }

        if (_sprites.ContainsKey(id))
        {
            return _sprites[id];
        }

        Debug.LogError($"could not find sprite with id '{id}'");
        return null;
    }

    private void LoadTexturesInternal(IEnumerable<string> textureIds, LoadMode mode)
    {
        foreach (var id in textureIds)
        {
            LoadTexture(id, mode);
        }
    }

    private void LoadTexture(string id, LoadMode mode)
    {
        // In append mode - skip over any images that are already loaded
        if (mode == LoadMode.Append && _sprites.ContainsKey(id))
            return;
            
        var image = UIImage.SystemImageNamed(id, _traits);
        if (image == null)
        {
            Debug.LogWarning($"no glyph found for name '{id}'");
            return;
        }
            
        Debug.Log($"loaded image for id: {id}");

        var texture = new Texture2D(1, 1);
        texture.LoadImage(UIImage.PNGRepresentation(image));

        _sprites[id]  = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height), 
            new Vector2(0.5f, 0.5f));
            
        image.Dispose();
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
    public void LoadAllControllerGlyphs(LoadFillOption loadFills, LoadMode loadMode = LoadMode.Replace)
    {
        // Code smell - refactor this line
        _symbolInfoCollection = SymbolInfoCollection.AllControllerSymbols;

        if (loadFills == LoadFillOption.LoadUnfilled || loadFills == LoadFillOption.LoadBoth)
        {
            LoadTexturesInternal(_symbolInfoCollection.Symbols.Select(g => g.symbolName), loadMode);
        }

        if (loadFills == LoadFillOption.LoadFilled || loadFills == LoadFillOption.LoadBoth)
        {
            var fillSymbolNames = SymbolInfoCollection.AllControllerSymbols.Symbols
                .Where(g => g.hasFill)
                .Select(g => g.filledSymbolName);
            LoadTexturesInternal(fillSymbolNames, loadMode);
        }
    }
}