using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceLister
{
    internal class GlobalGameManagersAssetsFile : AssetsFile
    {
        private ObjectInfo ResourceInfo => objectInfos.First(objectInfo => objectInfo.ClassID == 0x93);

        protected List<Resource> resources;

        internal string[] ResourceList => resources.Select(resource => resource.Path).ToArray();

        internal GlobalGameManagersAssetsFile() : base("globalgamemanagers") {
            reader.Seek(ResourceInfo.Start);
            var resourcesCount = reader.ReadInt32();
            resources = new List<Resource>(resourcesCount);
            for (int i = 0; i < resourcesCount; i++)
            {
                var pathLength = reader.ReadInt32();
                var resource = new Resource() { Path = Encoding.Default.GetString(reader.ReadBytes(pathLength)) };
                reader.Align(4);
                resource.FileID = reader.ReadInt32();
                resource.PathID = reader.ReadInt32();
                reader.Skip(4);
                resources.Add(resource);
                //Console.WriteLine($"{resource.Path}[{pathLength}]: {resource.FileID} / {resource.PathID}");
            }
        }

        internal class Resource
        {
            internal string Path;
            internal int FileID;
            internal int PathID;
        }
    }
}
