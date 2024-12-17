using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitObject : MonoBehaviour
{
    public GameObject fragmentPrefab; // Prefab for a sprite fragment
    public int numberOfFragments = 2; // Number of pieces to split into
    public Vector2 initialForceRange = new Vector2(2f, 2f); // Initial random force range for fragments
    public float spawnOffsetFactor = 0.5f; // Factor of sprite width for spacing

    private void Update()
    {
        OnSpace();
    }

    protected virtual void OnSpace()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyAndFragment();
        }
    }

    void DestroyAndFragment()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;

        // Get the original sprite texture and size
        Texture2D texture = spriteRenderer.sprite.texture;
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // Calculate the size of each fragment
        float fragmentWidth = spriteSize.x / numberOfFragments;
        float textureWidth = texture.width / numberOfFragments;

        for (int i = 0; i < numberOfFragments; i++)
        {
            // Calculate the fragment's position and offset
            float offsetX = (i - (numberOfFragments / 2f)) * fragmentWidth; // Center fragments around the original object
            Vector3 spawnPosition = transform.position + new Vector3(offsetX, 0, 0);

            // Create a fragment instance
            GameObject fragment = Instantiate(fragmentPrefab, spawnPosition, Quaternion.identity);

            // Adjust fragment's sprite
            SpriteRenderer fragmentRenderer = fragment.GetComponent<SpriteRenderer>();
            Rect fragmentRect = new Rect(i * textureWidth, 0, textureWidth, texture.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f); // Center pivot point for each fragment
            fragmentRenderer.sprite = Sprite.Create(texture, fragmentRect, pivot);

            // Add physics to the fragment
            Rigidbody2D rb = fragment.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Apply random force to scatter fragments
                Vector2 randomForce = new Vector2(
                    Random.Range(-initialForceRange.x, initialForceRange.x),
                    Random.Range(-initialForceRange.y, initialForceRange.y)
                );
                rb.AddForce(randomForce, ForceMode2D.Impulse);
            }
        }

        // Destroy the original object
        Destroy(gameObject);
    }
}
