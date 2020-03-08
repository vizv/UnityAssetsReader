// This file is part of the UnityAssetsReader <https://github.com/vizv/UnityAssetsReader/>.
// Copyright (C) 2020  Wenxuan Zhao
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Linq;
using System.Text;

namespace UnityAssetsReader
{
    public class DataReader
    {
        private readonly BinaryReader reader;
        public Endian Endianness = Endian.Big;

        internal DataReader(Stream stream) => reader = new BinaryReader(stream);

        internal void Seek(long start) => reader.BaseStream.Position = start;

        internal void Skip(int count) => Read(count);

        internal void Align(int count) => Skip((count - (int)(reader.BaseStream.Position % count)) % count);

        internal bool ReadBoolean() => reader.ReadBoolean();

        internal byte ReadByte() => Read(1).Byte;
        internal byte[] ReadBytes(int count) => Read(count).Bytes;

        internal ushort ReadUInt16LittleEndian() => Read(2).LittleEndian.UInt16;
        internal ushort ReadUInt16BigEndian() => Read(2).BigEndian.UInt16;
        internal ushort ReadUInt16() => Endianness == Endian.Little ? ReadUInt16LittleEndian() : ReadUInt16BigEndian();

        internal short ReadInt16LittleEndian() => Read(2).LittleEndian.Int16;
        internal short ReadInt16BigEndian() => Read(2).BigEndian.Int16;
        internal short ReadInt16() => Endianness == Endian.Little ? ReadInt16LittleEndian() : ReadInt16BigEndian();

        internal uint ReadUInt32LittleEndian() => Read(4).LittleEndian.UInt32;
        internal uint ReadUInt32BigEndian() => Read(4).BigEndian.UInt32;
        internal uint ReadUInt32() => Endianness == Endian.Little ? ReadUInt32LittleEndian() : ReadUInt32BigEndian();

        internal int ReadInt32LittleEndian() => Read(4).LittleEndian.Int32;
        internal int ReadInt32BigEndian() => Read(4).BigEndian.Int32;
        internal int ReadInt32() => Endianness == Endian.Little ? ReadInt32LittleEndian() : ReadInt32BigEndian();

        internal ulong ReadUInt64LittleEndian() => Read(8).LittleEndian.UInt64;
        internal ulong ReadUInt64BigEndian() => Read(8).BigEndian.UInt64;
        internal ulong ReadUInt64() => Endianness == Endian.Little ? ReadUInt64LittleEndian() : ReadUInt64BigEndian();

        internal long ReadInt64LittleEndian() => Read(8).LittleEndian.Int64;
        internal long ReadInt64BigEndian() => Read(8).BigEndian.Int64;
        internal long ReadInt64() => Endianness == Endian.Little ? ReadInt64LittleEndian() : ReadInt64BigEndian();

        internal string ReadString() => reader.ReadString();

        internal string ReadStringToZero()
        {
            var sb = new StringBuilder();
            for (var next = reader.ReadByte(); next > 0; next = reader.ReadByte()) sb.Append((char)next);
            return sb.ToString();
        }


        public enum Endian { Little, Big }

        private ByteArray Read(int count)
        {
            var bytes = reader.ReadBytes(count);
            return new ByteArray(Endian.Little, bytes);
        }

        internal class ByteArray
        {
            private readonly Endian endianness;
            internal readonly byte[] Bytes;

            internal ByteArray(Endian endianness, byte[] bytes)
            {
                Bytes = bytes;
                this.endianness = endianness;
            }


            internal ByteArray BigEndian => endianness == Endian.Big ? this : new ByteArray(Endian.Little, ReversedBytes);

            internal ByteArray LittleEndian => endianness == Endian.Little ? this : new ByteArray(Endian.Big, ReversedBytes);

            internal byte Byte => Bytes[0];

            internal ushort UInt16 => BitConverter.ToUInt16(Bytes, 0);

            internal short Int16 => BitConverter.ToInt16(Bytes, 0);

            internal uint UInt32 => BitConverter.ToUInt32(Bytes, 0);

            internal int Int32 => BitConverter.ToInt32(Bytes, 0);

            internal ulong UInt64 => BitConverter.ToUInt64(Bytes, 0);

            internal long Int64 => BitConverter.ToInt64(Bytes, 0);

            private byte[] ReversedBytes
            {
                get
                {
                    var reversedBytes = Bytes.ToArray();
                    Array.Reverse(reversedBytes);
                    return reversedBytes;
                }
            }
        }
    }
}
