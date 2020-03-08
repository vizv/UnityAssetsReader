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

using System;
using UnityAssetsReader;

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
