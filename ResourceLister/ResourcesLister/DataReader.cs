using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ResourceLister
{
    internal class DataReader
    {
        private readonly BinaryReader reader;
        public Endian Endianness = Endian.Big;

        internal DataReader(Stream stream)
        {
            reader = new BinaryReader(stream);
        }

        internal void Skip(int count) => Read(count);
        internal void Align(int count) => Skip((count - (int)(reader.BaseStream.Position % count)) % count);

        internal byte ReadByte() => Read(1).Byte;

        internal uint ReadUInt32LittleEndian() => Read(4).LittleEndian.UInt32;
        internal uint ReadUInt32BigEndian() => Read(4).BigEndian.UInt32;
        internal uint ReadUInt32() => Endianness == Endian.Little ? ReadUInt32LittleEndian() : ReadUInt32BigEndian();

        internal int ReadInt32LittleEndian() => Read(4).LittleEndian.Int32;
        internal int ReadInt32BigEndian() => Read(4).BigEndian.Int32;
        internal int ReadInt32() => Endianness == Endian.Little ? ReadInt32LittleEndian() : ReadInt32BigEndian();

        internal string ReadStringToZero()
        {
            var sb = new StringBuilder();
            for (var next = reader.ReadByte(); next > 0; next = reader.ReadByte()) sb.Append((char)next);
            return sb.ToString();
        }

        internal enum Endian { Little, Big }

        private ByteArray Read(int count)
        {
            var bytes = reader.ReadBytes(count);
            return new ByteArray(bytes, Endian.Little);
        }

        internal class ByteArray
        {
            private readonly byte[] bytes;
            private readonly Endian endianness;

            internal ByteArray(byte[] bytes, Endian endianness)
            {
                this.bytes = bytes;
                this.endianness = endianness;
            }


            internal ByteArray BigEndian => endianness == Endian.Big ? this : new ByteArray(ReversedBytes, Endian.Little);

            internal ByteArray LittleEndian => endianness == Endian.Little ? this : new ByteArray(ReversedBytes, Endian.Big);

            internal byte Byte => bytes[0];

            internal uint UInt32 => BitConverter.ToUInt32(bytes, 0);

            public int Int32 => BitConverter.ToInt32(bytes, 0);

            private byte[] ReversedBytes
            {
                get
                {
                    var reversedBytes = bytes.ToArray();
                    Array.Reverse(reversedBytes);
                    return reversedBytes;
                }
            }

        }
    }
}
