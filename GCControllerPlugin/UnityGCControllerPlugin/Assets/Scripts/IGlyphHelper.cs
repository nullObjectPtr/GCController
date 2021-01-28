using System.Collections.Generic;
using Rewired;
using UnityEngine;

public interface IGlyphHelper
{
    Sprite GetFirstGlyphForRewiredAction(Player player, int actionId);
    Sprite GetFirstGlyphForRewiredAction(Player player, string actionName);

    IEnumerable<Sprite> GetAllGlyphsForRewiredAction(Player player, int actionId);

    IEnumerable<Sprite> GetAllGlyphsForRewiredAction(Player player, string actionName);
}
