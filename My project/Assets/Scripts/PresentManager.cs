using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabObjects; // List of prefabs to spawn
    [SerializeField] private Transform spawnPoint; // Where the objects should be spawned
    [SerializeField] private float minDistanceToSpawn = 2f; // Minimum distance from the spawn point to spawn the next object
    [SerializeField] private int totalObjectsToSpawn = 10; // Total number of objects to spawn

    private List<GameObject> activePresents = new List<GameObject>(); // List to keep track of spawned objects
    private Queue<GameObject> spawnQueue = new Queue<GameObject>(); // Queue to track available prefabs

    void Start()
    {
        // Populate the spawnQueue with the prefabs (initially)
        foreach (var prefab in prefabObjects)
        {
            spawnQueue.Enqueue(prefab);
        }

        // Start the spawning process
        StartCoroutine(SpawnObjectCoroutine());
    }

    // Coroutine to handle spawning objects with distance checking
    private IEnumerator SpawnObjectCoroutine()
    {
        int spawnedCount = 0; // Track the number of objects spawned

        while (spawnedCount < totalObjectsToSpawn)
        {
            // Check if we can spawn a new object or a destroyed object
            if (CanSpawnNextObject())
            {
                SpawnObject();
                spawnedCount++; // Increment the count after a successful spawn
            }

            // Wait for a short time before checking again (adjust as needed)
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Check if the last spawned object is at least the required distance from the spawn point
    private bool CanSpawnNextObject()
    {
        // If there are no active objects, we can spawn the first one
        if (activePresents.Count == 0) return true;

        // Check the distance to the last spawned object
        GameObject lastSpawnedObject = activePresents[activePresents.Count - 1];
        if (Vector3.Distance(lastSpawnedObject.transform.position, spawnPoint.position) >= minDistanceToSpawn)
        {
            return true;
        }

        return false;
    }

    // Spawns an object randomly from the queue
    private void SpawnObject()
    {
        if (spawnQueue.Count > 0)
        {
            // Randomly select a prefab from the list
            GameObject prefabToSpawn = prefabObjects[Random.Range(0, prefabObjects.Count)];

            // Instantiate the object at the spawn point
            GameObject newObject = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

            // Add the spawned object to the active presents list
            activePresents.Add(newObject);

            // Optionally, track this object for destruction
            var destructible = newObject.GetComponent<DestructibleObject>();
            if (destructible != null)
            {
                destructible.OnDestroyed += ReturnObjectToQueue; // Subscribe to the destruction event
            }
        }
        else
        {
            Debug.Log("No objects left in the spawn queue.");
        }
    }

    // This method is called when an object is destroyed
    private void ReturnObjectToQueue(GameObject destroyedObject)
    {
        // Remove the destroyed object from the active list
        activePresents.Remove(destroyedObject);

        // Add the destroyed object back to the spawn queue for later respawning
        spawnQueue.Enqueue(destroyedObject);

        Debug.Log("Object returned to spawn queue.");
    }

    // Check periodically for destroyed objects that need to be respawned
    private void CheckForDestroyedObjectsAndRespawn()
    {
        // Check all objects in the queue (including destroyed ones) and spawn them if possible
        foreach (var destroyedObject in spawnQueue)
        {
            if (CanSpawnNextObject()) // Check if we can spawn the object based on distance
            {
                SpawnObject();
                return; // Spawn one object and exit after that
            }
        }
    }

    // Return the list of spawned objects to be used by CheckForPresents
    public List<GameObject> GetSpawnedObjects()
    {
        return activePresents;
    }
}