using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LocalPipe : MonoBehaviour, IOutputConnection, IInputConnection
{
    public int capacity;
    public float millisecondDelay;
    [Range(0,100)]
    public float loss;
    private MemoryStream _stream;

    private struct PacketRecord
    {
        public float sendTime;
        public byte[] bytes;

        public PacketRecord(float sendTime, byte[] bytes)
        {
            this.sendTime = Time.time + (sendTime / 1000f);
            this.bytes = bytes;
        }
    }

    private Queue<PacketRecord> packets = new Queue<PacketRecord>();
    
    public void OnEnable()
    {
        _stream = new MemoryStream(capacity);
    }
    public void SendData(byte[] data)
    {
        if (Random.value < loss / 100f)
            return;
        packets.Enqueue(new PacketRecord(Time.time + millisecondDelay, data));
    }

    public void FixedUpdate()
    {
        while (packets.Count > 0)
        {
            var record = packets.Peek();
            if (record.sendTime >= Time.time)
                break;

            record = packets.Dequeue();
            _stream.Write(record.bytes, 0, record.bytes.Length);
        }
    }

    public byte[] GetData()
    {
        var bytes = _stream.ToArray();
        _stream.Seek(0, SeekOrigin.Begin);
        _stream.SetLength(0);
        return bytes;
    }
}
