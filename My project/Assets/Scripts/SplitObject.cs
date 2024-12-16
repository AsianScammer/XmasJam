using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitObject : MonoBehaviour
{
    public GameObject fragmentPrefab; // Prefab for a sprite fragment
    public int numberOfFragments = 4; // Number of pieces to split into

    void Update()
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

        Texture2D texture = spriteRenderer.sprite.texture;
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // Calculate fragment size
        float fragmentWidth = spriteSize.x / numberOfFragments;

        for (int i = 0; i < numberOfFragments; i++)
        {
            // Create a fragment
            GameObject fragment = Instantiate(fragmentPrefab, transform.position, Quaternion.identity);

            // Adjust fragment's sprite
            SpriteRenderer fragmentRenderer = fragment.GetComponent<SpriteRenderer>();
            Rect fragmentRect = new Rect(i * fragmentWidth * texture.width, 0, fragmentWidth * texture.width, texture.height);
            fragmentRenderer.sprite = Sprite.Create(texture, fragmentRect, new Vector2(0.5f, 0.5f));
        }

        Destroy(gameObject); // Destroy original sprite
    }
}
