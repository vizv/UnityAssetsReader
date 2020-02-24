using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace ResourceLister
{
    internal class AssetsFile
    {
        protected DataReader reader;
        protected Header header;
        protected List<Type> types;
        protected List<ObjectInfo> objectInfos;

        public AssetsFile(string fileName)
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
                //Console.WriteLine($"{type.ClassID}, {type.IsStrippedType}, {type.ScriptTypeIndex}, {type.OldTypeHash}");
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
                //Console.WriteLine($"{objectInfo.PathID}, {objectInfo.Start}, {objectInfo.Size}, {objectInfo.TypeID}, {objectInfo.ClassID}");
            }

            int scriptsCount = reader.ReadInt32();
            Debug.Assert(scriptsCount == 0);
            // TODO: I don't care... for now

            int externalsCount = reader.ReadInt32();
            Debug.Assert(externalsCount > 0);
            // TODO: I don't care... for now
        }

        internal class Header
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

        internal class Type
        {
            internal int ClassID;
            internal bool IsStrippedType;
            internal short ScriptTypeIndex = -1;
            internal byte[] OldTypeHash; // Hash128
        }

        internal class ObjectInfo
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
