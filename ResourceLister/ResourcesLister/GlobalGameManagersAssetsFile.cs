using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceLister
{
    internal class GlobalGameManagersAssetsFile : AssetsFile
    {
        internal string[] ResourceList {
            get { return new string[0]; }
        }

        internal GlobalGameManagersAssetsFile() : base("globalgamemanagers.assets") { }
    }
}
