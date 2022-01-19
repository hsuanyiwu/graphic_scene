using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicScene
{
    public class NodeDeserillizer
    {
        private Dictionary<string, byte[]> _dict = new Dictionary<string, byte[]>();

        public NodeDeserillizer(byte[] bytes, int offset, int length)
        {
            var c = new Cursor(bytes, offset, length);
            while (c.index < c.length)
            {
                string key = c.ReadString();
                byte[] val = c.ReadBytes();
                _dict.Add(key, val);
            }
        }
        public NodeDeserillizer(byte[] bytes)
            : this(bytes, 0, bytes.Length)
        {

        }

        public int ReadInt32(string key)
        {
            return BitConverter.ToInt32(_dict[key], 0);
        }

        public string ReadString(string key)
        {
            return Encoding.UTF8.GetString(_dict[key]);
        }

        public byte[] ReadBytes(string key)
        {
            return _dict[key];
        }

        public void Read(string key, out NodeDeserillizer s)
        {
            s = new NodeDeserillizer(_dict[key]);
        }

        public IEnumerable<NodeDeserillizer> EnumList(string key)
        {
            var c = new Cursor(_dict[key]);
            int n = c.ReadInt32();
            for (int i = 0; i < n; ++i)
            {
                int len = c.ReadInt32();
                yield return new NodeDeserillizer(c.bytes, c.index, c.index + len);
                c.index += len;
            }
        }

        class Cursor
        {
            public byte[] bytes;
            public int index;
            public int length;
            public Cursor(byte[] bytes, int offset, int length)                
            {
                this.bytes = bytes;
                this.index = offset;
                this.length = length;
            }
            public Cursor(byte[] bytes)
                : this(bytes, 0, bytes.Length)
            {

            }

            public string ReadString()
            {
                int len = ReadInt32();
                string str = Encoding.UTF8.GetString(bytes, index, len);
                index += len;
                return str;
            }

            public int ReadInt32()
            {
                int i = BitConverter.ToInt32(bytes, index);
                index += 4;
                return i;
            }

            public byte[] ReadBytes()
            {
                int len = ReadInt32();
                byte[] ret = new byte[len];
                Array.Copy(bytes, index, ret, 0, len);
                index += len;
                return ret;
            }
        }
    }
}
