using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fier : MonoBehaviour
{
    [SerializeField] private Light light;
    [SerializeField] private float amplitude = 1;
    [SerializeField] private float speed = 1;

    private float startRange;
    private float startIntensity;

    float qewr;

    private void Start()
    {
        startRange = light.range;
        startIntensity = light.intensity;
    }

    void Update()
    {
        light.range = startRange + Mathf.Sin(Time.time * speed) * amplitude;
        light.intensity = startIntensity + Mathf.Sin(Time.time * speed) * amplitude;
    }
}