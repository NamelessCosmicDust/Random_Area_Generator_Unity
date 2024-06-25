using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a static class that includes all other helpful functions to generate game play areas.
/// </summary>
public static class AreaGenerationHelpers
{
    /// <summary>
    /// Process an Area GameObject and return a transform of its SpawnPoint child.
    /// </summary>
    /// <param name="parentArea"></param>
    /// <returns></returns>
    public static Transform GetSpawnPointTransform(GameObject parentArea)
    {
        Transform[] children = parentArea.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.CompareTag("SpawnPoint"))
            {
                return child;
            }
        }
        throw new System.Exception("SpawnPoint tag not found in children of " + parentArea.name);
    }

    /// <summary>
    /// Process an Area GameObject and return a list of transforms of its SpawnPoint children.
    /// </summary>
    /// <param name="parentArea"></param>
    /// <returns></returns>
    public static List<Transform> GetSpawnPointTransforms(GameObject parentArea)
    {
        Transform[] children = parentArea.GetComponentsInChildren<Transform>();
        List<Transform> spawnPoints = new();

        foreach (Transform child in children)
        {
            if (child.CompareTag("SpawnPoint"))
            {
                spawnPoints.Add(child);
            }
        }

        if (spawnPoints.Count == 0)
        {
            throw new System.Exception("No SpawnPoint tags found in children of " + parentArea.name);
        }

        return spawnPoints;
    }

    /// <summary>
    /// Rotate the input vector by rotationAngle, starting from startAngle
    /// and return new vectors upto the number equal to maxRotation.
    /// </summary>
    /// <param name="spawnPointVector"></param>
    /// <param name="maxRotation"></param>
    /// <param name="rotationAngle"></param>
    /// <param name="startAngle"></param>
    /// <returns></returns>
    public static Vector2[] GetSurroundingPositions
        (Vector2 spawnPointVector, float rotationAngle, float startAngle, int maxRotation)
    {
        Vector2[] positions = new Vector2[maxRotation];
        spawnPointVector = spawnPointVector.normalized;

        for (int i = 0; i < maxRotation; i++)
        {
            float targetAngle = startAngle + rotationAngle * i;

            if (i % 2 != 0)
            {
                Vector2 newPositionVector = spawnPointVector * Mathf.Sqrt(200f);
                positions[i] = Quaternion.Euler(0, 0, targetAngle) * newPositionVector;
            }
            else
            {
                Vector2 newPositionVector = spawnPointVector * 10;
                positions[i] = Quaternion.Euler(0, 0, targetAngle) * newPositionVector;
            }
        }
        return positions;
    }

}
