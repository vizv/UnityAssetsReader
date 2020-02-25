using System;
using AssetsReader;

namespace ResourceLister
{
    class Program
    {
        static void Main()
        {
            var globalGameManagersAssetsFile = new GlobalGameManagersAssetsFile("globalgamemanagers");
            Console.WriteLine(string.Join("\n", globalGameManagersAssetsFile.ResourceList));
            Console.ReadKey();
        }
    }
}
