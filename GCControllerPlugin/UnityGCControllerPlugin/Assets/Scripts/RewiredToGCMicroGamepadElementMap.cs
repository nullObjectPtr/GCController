// Maps a rewired controller element name to a GCMicroGamepad controller element

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Rewired to Micro Gamepad Element Map")]
public class RewiredToGCMicroGamepadElementMap : ScriptableObject
{
    [Serializable]
    public class Record
    {
        public string RewiredElementName;
        public GCMicroGamepadElementType microGamepadElementType;
    }

    public Record[] Records;
}