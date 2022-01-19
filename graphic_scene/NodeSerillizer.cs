using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicScene
{
    public class NodeSerillizer
    {
        public List<byte> data = new List<byte>();

        public void Clear()
        {
            data.Clear();
        }

        private void WriteInt32(int value)
        {
            data.AddRange(BitConverter.GetBytes(value));
        }

        private void WriteString(string str)
        {
            WriteBytes(Encoding.UTF8.GetBytes(str));
        }

        private void WriteBytes(byte[] bytes)
        {
            WriteInt32(bytes.Length);
            if (bytes.Length > 0)
                data.AddRange(bytes);
        }

        public void Write(string key, string value)
        {
            Write(key, Encoding.UTF8.GetBytes(value));
        }

        public void Write(string key, int value)
        {
            Write(key, BitConverter.GetBytes(value));
        }

        public void Write(string key, byte[] bytes)
        {
            WriteString(key);
            WriteBytes(bytes);
        }

        public void Write(string key, NodeSerillizer value)
        {
            WriteString(key);
            WriteBytes(value.data.ToArray());
        }

        public void Write(string key, List<NodeSerillizer> value)
        {
            var temp = new NodeSerillizer();
            temp.WriteInt32(value.Count);
            foreach (var i in value)
            {
                temp.WriteBytes(i.data.ToArray());
            }
            Write(key, temp);
        }
    }

    /*class NodeSerillizer_Binary : NodeSerillizer
    {

    }

    class NodeSerillizer_Json : NodeSerillizer
    {

    }*/


}
