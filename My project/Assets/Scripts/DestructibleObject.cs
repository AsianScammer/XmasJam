using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public event System.Action<GameObject> OnDestroyed; // Event triggered when the object is destroyed

    private void OnDestroy()
    {
        // Notify the PresentManager that the object was destroyed
        OnDestroyed?.Invoke(gameObject);
        Debug.Log("Object destroyed: " + gameObject.name);
    }
}