using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOutputConnection
{
    void SendData(byte[] data);
}

public interface IInputConnection
{
    byte[] GetData();
}
