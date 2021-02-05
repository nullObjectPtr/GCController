using System;
using UnityEngine;

/// <summary>
/// This class maps the names of GCController elements to sprite names loaded by a SFSymbolSet
/// </summary>
///
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GCControllerElement to SFSymbolName Map")]
public class GCControllerElementToSFSymbolNameMap : ScriptableObject
{
    [Serializable]
    public class Record
    {
        public string elementType;
        public string sfSymbolName;
    }

    public Record[] Records;
}
