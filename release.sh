#!/bin/sh
cd Assemblies
rm Assembly-CSharp* Mono.* mscorlib.dll System.* TextMeshPro-1.0.55.56.0b11.dll UnityEngine.* 0Harmony.dll HugsLib.dll ShelfRenamer.dll.mdb
cd ../..
zip -r ShelfRenamer ShelfRenamer/About ShelfRenamer/Assemblies ShelfRenamer/CREDITS ShelfRenamer/Defs ShelfRenamer/LICENSE ShelfRenamer/README.md ShelfRenamer/Source ShelfRenamer/Textures
