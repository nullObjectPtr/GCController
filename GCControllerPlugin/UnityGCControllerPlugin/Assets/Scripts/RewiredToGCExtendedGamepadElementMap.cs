using System;
using UnityEngine;

// Maps a rewired controller element name to a GCExtendedGamepad controller element
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Rewired to Extended Gamepad Element Map")]
public class RewiredToGCExtendedGamepadElementMap : ScriptableObject
{
    [Serializable]
    public class Record
    {
        public string RewiredElementName;
        public GCExtendedGamepadElementType extendedGamepadElementType;
    }

    public Record[] Records;
}