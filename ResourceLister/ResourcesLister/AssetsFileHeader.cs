using System;

namespace ResourceLister
{
    internal class AssetsFileHeader
    {
        public uint MetadataSize;
        public uint FileSize;
        public uint Version;
        public uint DataOffset;
    }
}