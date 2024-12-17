using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForPresents : MonoBehaviour
{
    private List<Transform> objectsInCollider = new List<Transform>(); // List of objects inside the collider
    private List<Transform> expectedObjects = new List<Transform>(); // List of objects expected to be inside the collider

    private bool allObjectsInside = false;

    private Throwing throwingScript; // Reference to the Throwing script
    private PresentManager presentManager; // Reference to the PresentManager script

    private void Start()
    {
        // Find the Throwing and PresentManager scripts in the scene
        throwingScript = FindObjectOfType<Throwing>();
        presentManager = FindObjectOfType<PresentManager>();

        // Subscribe to the event from PresentManager when objects are spawned
        if (presentManager != null)
        {
            // Populate the expectedObjects list with spawned objects from PresentManager
            foreach (var present in presentManager.GetSpawnedObjects())
            {
                expectedObjects.Add(present.transform);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore if object is being held
        if (IsObjectHeld(other.attachedRigidbody)) return;

        // Check if the object is expected and not already inside the collider
        if (expectedObjects.Contains(other.transform) && !objectsInCollider.Contains(other.transform))
        {
            objectsInCollider.Add(other.transform);

            if (AreAllObjectsInside() && !allObjectsInside)
            {
                allObjectsInside = true;
                StartCoroutine(WaitAndLockObjects(5f)); // Wait for a specified time before locking
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (objectsInCollider.Contains(other.transform))
        {
            objectsInCollider.Remove(other.transform);
            allObjectsInside = false;
        }
    }

    private bool AreAllObjectsInside()
    {
        // Check that all expected objects are inside the collider and not being held
        foreach (Transform obj in expectedObjects)
        {
            if (!objectsInCollider.Contains(obj) && !IsObjectHeld(obj.GetComponent<Rigidbody2D>()))
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator WaitAndLockObjects(float delay)
    {
        Debug.Log("All objects are inside. Waiting for " + delay + " seconds...");
        yield return new WaitForSeconds(delay);

        if (AreAllObjectsInside())
        {
            ParentAndLockObjects();
        }
        else
        {
            Debug.Log("Some objects exited before lock. Cancelled.");
            allObjectsInside = false;
        }
    }

    private void ParentAndLockObjects()
    {
        // Lock all objects that are inside the collider
        foreach (Transform objTransform in objectsInCollider)
        {
            if (objTransform != null && !IsObjectHeld(objTransform.GetComponent<Rigidbody2D>()))
            {
                objTransform.SetParent(transform);

                Rigidbody2D rb = objTransform.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                    rb.constraints = RigidbodyConstraints2D.FreezeAll; // Lock the object in place
                }
            }
        }
        Debug.Log("All objects have been parented and locked in place!");
    }

    private bool IsObjectHeld(Rigidbody2D rb)
    {
        // Check if the Throwing script is holding this object
        return throwingScript != null && throwingScript.HeldObject == rb;
    }

    // This method can be used to update the expected objects in the CheckForPresents script
    public void AddObjectToExpectedList(Transform newObject)
    {
        if (!expectedObjects.Contains(newObject))
        {
            expectedObjects.Add(newObject);
        }
    }

    // This method can be used to remove objects from the expected list if needed
    public void RemoveObjectFromExpectedList(Transform objectToRemove)
    {
        if (expectedObjects.Contains(objectToRemove))
        {
            expectedObjects.Remove(objectToRemove);
        }
    }
}
