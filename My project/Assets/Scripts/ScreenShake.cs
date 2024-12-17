using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Transform cameraTransform; // Zorg ervoor dat dit de camera bevat die je wilt schudden.
    public float shakeDuration = 0.5f; // Hoe lang het schudden duurt.
    public float shakeMagnitude = 0.2f; // Hoe sterk de schudbeweging is.

    private Vector3 originalPosition; // De originele positie van de camera.
    private float shakeTimeRemaining; // De resterende tijd van het schudden.

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Standaard naar de main camera als niets is ingesteld.
        }
        originalPosition = cameraTransform.localPosition;
    }

    void Update()
    {
        // Start screenshake bij spatiebalk
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartShake();
        }

        // Als er nog shake-tijd over is, voer de shake uit
        if (shakeTimeRemaining > 0)
        {
            cameraTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;
        }
        else if (shakeTimeRemaining <= 0 && cameraTransform.localPosition != originalPosition)
        {
            // Reset de camera naar de originele positie als het schudden klaar is
            cameraTransform.localPosition = originalPosition;
        }
    }

    public void StartShake()
    {
        shakeTimeRemaining = shakeDuration;
    }
}
