using System;
using System.IO;

namespace ResourceLister
{
    internal class AssetsFile
    {
        protected DataReader reader;
        protected AssetsFileHeader header;

        public AssetsFile(string fileName)
        {
            reader = new DataReader(File.OpenRead(fileName));

            header = new AssetsFileHeader
            {
                MetadataSize = reader.ReadUInt32BigEndian(),
                FileSize = reader.ReadUInt32BigEndian(),
                Version = reader.ReadUInt32BigEndian(),
                DataOffset = reader.ReadUInt32BigEndian(),
                Endianness = reader.ReadByte(),
            };

            reader.Align(4);
            reader.Endianness = (DataReader.Endian)header.Endianness;
            header.UnityVersion = reader.ReadStringToZero();
            header.TargetPlatform = reader.ReadInt32();

            Console.WriteLine(header.TargetPlatform);
            Console.ReadKey();
        }
    }
}
