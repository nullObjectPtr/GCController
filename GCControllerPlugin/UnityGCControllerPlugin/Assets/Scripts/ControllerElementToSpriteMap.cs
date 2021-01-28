using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Fallback Glyph Map")]
public class ControllerElementToSpriteMap : ScriptableObject
{
    [Serializable]
    public class Entry
    {
        public string ElementName;
        public Sprite Sprite;
    }

    public Entry[] Entries;

    public Sprite GetSpriteForElement(string elementName)
    {
        var entry = Entries.FirstOrDefault(e => e.ElementName == elementName);
        return entry?.Sprite;
    }
}
