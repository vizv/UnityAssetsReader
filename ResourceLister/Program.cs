using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceLister
{
    class Program
    {
        static void Main()
        {
            var globalGameManagersAssetsFile = new GlobalGameManagersAssetsFile();
            Console.WriteLine(string.Join("\n", globalGameManagersAssetsFile.ResourceList));
        }
    }
}
