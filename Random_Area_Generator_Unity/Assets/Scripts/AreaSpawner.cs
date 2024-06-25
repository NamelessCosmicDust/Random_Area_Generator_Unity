using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// AreaSpawner is attached to a spawn-point GameObject
/// which will be used to spawn a next area.
/// </summary>
public class AreaSpawner : MonoBehaviour
{
	public int openingDirection;
	// 1 = Top open, need bottom open
	// 2 = Right open, need left open
	// 3 = Botton open, need top open
	// 4 = Left open, need right open
    public bool spawned = false; // To ensure that a spawner is used only once.

	private AreaManager areaManager;
    private Vector2 thisPosition;
    private Vector2 parentPosition;

    void Start()
	{
        thisPosition = transform.position;
        parentPosition = transform.parent.position;
        Debug.Log("This position: " + thisPosition + "Parent position: " +parentPosition);

        // Assign existing AreaManager gameObject in the scene to this instance
        // to keep all the changes in the same instance of AreaManager.
        areaManager = GameObject.FindGameObjectWithTag("PlayArea").GetComponent<AreaManager>();

		SpawnArea();

        // After everything is done, destroy this spawner.
        Destroy(gameObject);
	}

	void SpawnArea()
	{
        // Spawn an area object only if the space is empty.
        bool isAreaOccupied = IsAreaOccupied(thisPosition);
		if (!isAreaOccupied && spawned == false)
		{
            // Spawn a regular area if the total number of areas are less than the AreaManager.areaSize.
            if (areaManager.areaCoordinates.Count < areaManager.areaSize)
            {
                GameObject[] areas;
                switch (openingDirection)
                {
                    case 1:
                        areas = areaManager.bottomOpenAreas;
                        break;
                    case 2:
                        areas = areaManager.leftOpenAreas;
                        break;
                    case 3:
                        areas = areaManager.topOpenAreas;
                        break;
                    case 4:
                        areas = areaManager.rightOpenAreas;
                        break;
                    default:
                        return;
                }

                #region Initial selection of an area
                HashSet<int> areaIndex = new();
                for (int i = 0; i < areas.Length; i++)
                {
                    areaIndex.Add(i);
                }
                // Pick a random index.
                int randIndex = areaIndex.ElementAt(UnityEngine.Random.Range(0,areaIndex.Count));
                areaIndex.Remove(randIndex); // Remove used index.
                GameObject areaToSpawn = areas[randIndex];
                #endregion

                // Before placing the selected area, make sure it won't open to an occupied area.
                while (IsNewPathBlocked(areaToSpawn, parentPosition))
                {
                    // If selected area is not valid type, reroll.
                    randIndex = areaIndex.ElementAt(UnityEngine.Random.Range(0, areaIndex.Count));
                    areaIndex.Remove(randIndex); // Remove used index.

                    // Assign new area and test the loop condition again.
                    areaToSpawn = areas[randIndex];

                    if (areaIndex.Count <= 0)
                    {
                        break; // Exit the loop when reaching the last option.
                    }
                }

                // If all good, Instantiate.
                Instantiate(areaToSpawn, thisPosition, areaToSpawn.transform.rotation);
            }
            else // Once reach the max number of area, spawn end areas to cap any open ends.
            {
                GameObject area;
                switch (openingDirection)
                {
                    case 1:
                        area = areaManager.bottomEndAreas;
                        break;
                    case 2:
                        area = areaManager.leftEndAreas;
                        break;
                    case 3:
                        area = areaManager.topEndAreas;
                        break;
                    case 4:
                        area = areaManager.rightEndAreas;
                        break;
                    default:
                        return;
                }
                Instantiate(area, thisPosition, area.transform.rotation);

            }

            // Add coord of this object to AreaManager.areaCoordinates.
            areaManager.areaCoordinates.Add(thisPosition);
		}		
        spawned = true; // Spawned an area or not, this spawner has been used.
	}

    /// <summary>
    /// This Method will check if the coordinate of the input area is already in AreaManager.areaCoordinates.
    /// Unity Instantiate() does not create a GameObject at the exact coordinate.
    /// Thus, this method will match anything within distance of 4.
    /// </summary>
    /// <param name="nextSpawnPosition"></param>
    /// <returns></returns>
    bool IsAreaOccupied(Vector2 nextSpawnPosition)
    {
        foreach (Vector2 areaCoord in areaManager.areaCoordinates)
        {
            // Calculate the squared distance between nextSpawnPosition and areaCoord.
            float sqrDistance = (nextSpawnPosition - areaCoord).sqrMagnitude;

            // Check if the squared distance is within the specified units.
            if (sqrDistance <= 16) // Within 4 units. 4 * 4 = 16
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Iterate through all next spawn points of the input area GameObject and
    /// check if any of them has a blocked path forward.
    /// This method requires IsAreaOccupied Method.
    /// </summary>
    /// <param name="areaToSpawn"></param>
    /// <param name="parentVector"></param>
    /// <returns></returns>
    bool IsNewPathBlocked(GameObject areaToSpawn, Vector2 parentVector)
    {
        List<Transform> nextSpawnPoints = AreaGenerationHelpers.GetSpawnPointTransforms(areaToSpawn);

        foreach (Transform nextSpawnPoint in nextSpawnPoints)
        {
            // Since nextSpawnPoint has not been Instantiated, its vector origin is always zero.
            // Thus, thisPosition has to be added to find the vector on the game scene.
            Vector2 nextSpawnVector =
                new Vector2(nextSpawnPoint.position.x, nextSpawnPoint.position.y) + thisPosition;
            float sqrDistance = (nextSpawnVector - parentVector).sqrMagnitude;

            if (sqrDistance <= 16) // Within 4 units. 4 * 4 = 16
            {
                // if 
                continue;
            }

            if (IsAreaOccupied(nextSpawnVector))
            {
                return true;
            }
        }
        return false;
    }

}
