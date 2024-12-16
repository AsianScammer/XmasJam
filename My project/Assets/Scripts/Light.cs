using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Light : MonoBehaviour
{
    Light2D light;
    [SerializeField] float intensityMin = 2f;
    [SerializeField] float intensityMax = 14f;
    [SerializeField] float speed = 0.5f;

    private void Start()
    {
        light = GetComponent<Light2D>();
    }
    private void Update()
    {
        float i = Mathf.PingPong(Time.time * speed, 1);

        light.intensity = Mathf.Lerp(intensityMin, intensityMax, i);
    }
}
