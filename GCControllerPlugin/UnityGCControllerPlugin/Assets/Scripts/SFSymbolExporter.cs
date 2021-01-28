using System;
using System.Collections.Generic;
using System.IO;
using HovelHouse.GameController;
using UnityEngine;

public class SFSymbolExporter
{
    public static void ExportPNGS(IEnumerable<string> names, double pointSize, string directoryToSaveTo)
    {
        ExportPNGS(names, pointSize, false, directoryToSaveTo);
    }

    public static void ExportPNGS(
        IEnumerable<string> symbolNames, 
        double pointSize, 
        bool forceSquare, 
        string directoryToSaveTo)
    {
        foreach (var weight in Enum.GetValues(typeof(UIImageSymbolWeight)))
        {
            var newDirectoryToSaveTo = Path.Combine(directoryToSaveTo, $"./{weight}");
            ExportPNGS(symbolNames, pointSize, (UIImageSymbolWeight)weight, forceSquare, newDirectoryToSaveTo);
        }
    }

    public static void ExportPNGS(
        IEnumerable<string> symbolNames, 
        double pointSize, 
        UIImageSymbolWeight weight, 
        bool forceSquare,
        string directoryToSaveTo)
    {
        var texture2D = new Texture2D(1,1);
        var resizedTexture = new Texture2D(1, 1);

        var symbolConfiguration = UIImageSymbolConfiguration
            .ConfigurationWithPointSize(pointSize, weight, UIImageSymbolScale.Large);
          
        Directory.CreateDirectory(directoryToSaveTo);

        foreach (var symbolName in symbolNames)
        {
            var image = UIImage.SystemImageNamed(symbolName, symbolConfiguration);

            if (image == null)
                continue;
                
            var filepath = Path.Combine(directoryToSaveTo, $"./{symbolName}.png");
                
            Debug.Log($"saving {symbolName} to {filepath}");
                
            var bytes = UIImage.PNGRepresentation(image);

            if (forceSquare)
            {
                texture2D.LoadImage(bytes);
                var newSize = CeilPow2((uint) Mathf.Max(texture2D.width, texture2D.height));
                resizedTexture = FixGlyphTexture(texture2D, (int) newSize);
                var resizedBytes = resizedTexture.EncodeToPNG();
                File.WriteAllBytes(filepath, resizedBytes);
            }
            else
            {
                File.WriteAllBytes(filepath, bytes);
            }
        }
    }
    
    static Texture2D FixGlyphTexture(Texture2D rawGlyphTexture, int size)
    {
        var newTexture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(rawGlyphTexture, 0, 0, 0, 0, rawGlyphTexture.width, rawGlyphTexture.height, newTexture, 0, 0, 
            newTexture.width/2 - rawGlyphTexture.width/2,
            newTexture.height/2 - rawGlyphTexture.height/2);
	    
        return newTexture;
    }

    /// <summary>
    /// Rounds a number up to the closest power of 2
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    static uint CeilPow2(uint v)
    {
        v--;
        v |= v >> 1;
        v |= v >> 2;
        v |= v >> 4;
        v |= v >> 8;
        v |= v >> 16;
        v++;

        return v;
    }
}
