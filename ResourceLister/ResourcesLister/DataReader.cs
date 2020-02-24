using System;
using System.IO;
using System.Linq;

namespace ResourceLister
{
    internal class DataReader
    {
        private readonly BinaryReader reader;

        internal DataReader(Stream stream)
        {
            reader = new BinaryReader(stream);
        }

        internal uint ReadUInt32BigEndian() => Read(4).BigEndian.UInt32;

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

            internal uint UInt32 => BitConverter.ToUInt32(bytes, 0);

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
