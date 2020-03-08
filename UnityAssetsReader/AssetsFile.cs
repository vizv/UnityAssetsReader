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

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace UnityAssetsReader
{
    public class AssetsFile
    {
        protected DataReader reader;
        protected Header header;
        protected List<Type> types;
        protected List<ObjectInfo> objectInfos;

        internal AssetsFile(string fileName)
        {
            reader = new DataReader(File.OpenRead(fileName));

            header = new Header
            {
                MetadataSize = reader.ReadUInt32BigEndian(),
                FileSize = reader.ReadUInt32BigEndian(),
                Version = reader.ReadUInt32BigEndian(),
                DataOffset = reader.ReadUInt32BigEndian(),
                Endianness = reader.ReadByte(),
                Unused = reader.ReadBytes(3),
                UnityVersion = reader.ReadStringToZero(),
            };
            Debug.Assert(header.Version == 17);
            Debug.Assert(header.Endianness == 0);
            Debug.Assert(header.UnityVersion == "2018.4.12f1");
            reader.Endianness = (DataReader.Endian)header.Endianness;
            header.TargetPlatform = reader.ReadInt32();
            header.EnableTypeTree = reader.ReadBoolean();
            Debug.Assert(header.TargetPlatform == 19);
            Debug.Assert(header.EnableTypeTree == false);

            var typesCount = reader.ReadInt32();
            types = new List<Type>(typesCount);
            for (int i = 0; i < typesCount; i++)
            {
                var type = new Type()
                {
                    ClassID = reader.ReadInt32(),
                    IsStrippedType = reader.ReadBoolean(),
                    ScriptTypeIndex = reader.ReadInt16(),
                    OldTypeHash = reader.ReadBytes(16),
                };
                types.Add(type);
            }

            int objectsCount = reader.ReadInt32();
            objectInfos = new List<ObjectInfo>(objectsCount);
            for (int i = 0; i < objectsCount; i++)
            {
                reader.Align(4);
                var objectInfo = new ObjectInfo(this)
                {
                    PathID = reader.ReadInt64(),
                    Start = header.DataOffset + reader.ReadUInt32(),
                    Size = reader.ReadUInt32(),
                    TypeID = reader.ReadInt32(),
                };
                objectInfos.Add(objectInfo);
            }

            int scriptsCount = reader.ReadInt32();
            Debug.Assert(scriptsCount == 0);
            // TODO: I don't care... for now

            int externalsCount = reader.ReadInt32();
            Debug.Assert(externalsCount > 0);
            // TODO: I don't care... for now
        }

        protected class Header
        {
            internal uint MetadataSize;
            internal uint FileSize;
            internal uint Version;
            internal uint DataOffset;
            internal byte Endianness;
            internal byte[] Unused;
            internal string UnityVersion;
            internal int TargetPlatform;
            internal bool EnableTypeTree;
        }

        protected class Type
        {
            internal int ClassID;
            internal bool IsStrippedType;
            internal short ScriptTypeIndex = -1;
            internal byte[] OldTypeHash; // Hash128
        }

        protected class ObjectInfo
        {
            internal AssetsFile AssetsFile { get; }

            internal long PathID;
            internal uint Start;
            internal uint Size;
            internal int TypeID;

            internal Type Type => AssetsFile.types[TypeID];
            internal int ClassID => Type.ClassID;

            internal ObjectInfo(AssetsFile assetsFile)
            {
                AssetsFile = assetsFile;
            }
        }
    }
}
