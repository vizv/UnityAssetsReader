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
using System.Linq;
using System.Text;

namespace UnityAssetsReader
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
