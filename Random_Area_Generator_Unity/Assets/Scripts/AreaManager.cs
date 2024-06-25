using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AreaManager holds all possible area types
/// and also keeps the count and positions of all areas created.
/// </summary>
public class AreaManager : MonoBehaviour
{
    // start and end Area have one way opening.
	public GameObject[] startAreas;
    public GameObject topEndAreas;
    public GameObject bottomEndAreas;
    public GameObject leftEndAreas;
    public GameObject rightEndAreas;
    public GameObject blockedArea;
    
    // other areas have at least two-way opening.
	public GameObject[] topOpenAreas;
	public GameObject[] bottomOpenAreas;
	public GameObject[] leftOpenAreas;
	public GameObject[] rightOpenAreas;

    public List<Vector2> areaCoordinates;
	public int areaSize; // areaSize is used to limit number of area creation.

    private void Start()
    {
		areaSize = 20;

		// Create the first area using one of terminalAreas.
        int rand = Random.Range(0, startAreas.Length);
		GameObject thisArea =
			Instantiate(startAreas[rand], Vector2.zero, startAreas[rand].transform.rotation);
		// Then, add the location of the first area to the list.
        areaCoordinates.Add(thisArea.transform.position);

        // Reserve 5 positions around the first area
        // to prevent any new area spawns right next to the first area.
        Transform spawnPoint = AreaGenerationHelpers.GetSpawnPointTransform(thisArea);
        Vector2[] positions =
            AreaGenerationHelpers.GetSurroundingPositions(spawnPoint.position, 45f, 90f, 5);

        foreach (Vector2 position in positions)
        {
            Instantiate(blockedArea, position, Quaternion.identity);
            areaCoordinates.Add(position);
        }
    }
}
