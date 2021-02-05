using System.Collections.Generic;
using Rewired;
using UnityEngine;

public interface IGlyphHelper
{
    Sprite GetFirstGlyphForRewiredAction(Player player, int actionId, bool filled);
    Sprite GetFirstGlyphForRewiredAction(Player player, string actionName, bool filled);

    IEnumerable<Sprite> GetAllGlyphsForRewiredAction(Player player, int actionId, bool filled);

    IEnumerable<Sprite> GetAllGlyphsForRewiredAction(Player player, string actionName, bool filled);
}
