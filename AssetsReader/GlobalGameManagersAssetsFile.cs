using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetsReader
{
    public class GlobalGameManagersAssetsFile : AssetsFile
    {
        private ObjectInfo ResourceInfo => objectInfos.First(objectInfo => objectInfo.ClassID == 0x93);

        private readonly List<Resource> resources;

        public string[] ResourceList => resources.Select(resource => resource.Path).ToArray();

        public GlobalGameManagersAssetsFile(string fileName) : base(fileName) {
            reader.Seek(ResourceInfo.Start);
            var resourcesCount = reader.ReadInt32();
            resources = new List<Resource>(resourcesCount);
            for (int i = 0; i < resourcesCount; i++)
            {
                var pathLength = reader.ReadInt32();
                var resource = new Resource() { Path = Encoding.Default.GetString(reader.ReadBytes(pathLength)) };
                reader.Align(4);
                resource.FileID = reader.ReadInt32();
                resource.PathID = reader.ReadInt64();
                resources.Add(resource);
                //Console.WriteLine($"{resource.Path}[{pathLength}]: {resource.FileID} / {resource.PathID}");
            }
        }

        public class Resource
        {
            internal string Path;
            internal int FileID;
            internal long PathID;
        }
    }
}
