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
                DataOffset = reader.ReadUInt32BigEndian()
            };
        }
    }
}
