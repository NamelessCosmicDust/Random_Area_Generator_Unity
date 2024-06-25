# Random_Area_Generator_Unity
A random area generator (intended for ARPG) for Unity Engine and my first ever public repository!

This is not totally my own idea. I watched a video tutorial by BlackthronProd and downloaded his code to learn how he did.
I did not want to rely on Unity colliders as he did and wanted to try a different way and this is it. But honestly, using colliders makes things easier...

Anyway, to use this, create an empty gameObject in a scene, then attach AreaManager.cs to it.
Then add prefabs to the right slots in Unity Editor.

AreaManager will Instantiate() an area which has a spawn point that will run AreaSpawner.cs.
Each AreaSpawner will look at total number of areas that have been placed (AreaManager - areaSize) and halt generation eventually.

Known issue: AreaSpawner also looks at the path forward and if selected area type has an opening to a blocked area, select a different area to prevent adding an opening to a blocked area. However, an existing opening still can be blocked by a new area. There is no check for this.
