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
            get { throw new NotImplementedException(); }
        }

        internal GlobalGameManagersAssetsFile() : base("globalgamemanagers.assets") { }
    }
}
