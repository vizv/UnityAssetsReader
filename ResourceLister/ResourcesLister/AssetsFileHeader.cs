using System;

namespace ResourceLister
{
    internal class AssetsFileHeader
    {
        internal uint MetadataSize;
        internal uint FileSize;
        internal uint Version;
        internal uint DataOffset;
        internal byte Endianness;
        internal string UnityVersion;
        internal int TargetPlatform;
    }
}