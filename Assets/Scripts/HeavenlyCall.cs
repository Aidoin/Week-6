using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavenlyCall : MonoBehaviour
{
    [SerializeField] private ParticleSystem riseRing;
    [SerializeField] private ParticleSystem light;
    [SerializeField] private ParticleSystem light2;
    [SerializeField] private ParticleSystem coreLight;
    [SerializeField] private ParticleSystem flash;

    public bool e;


    private void Update()
    {
        if(e)
        {
            StartParticle();
        }
    }

    public void StartParticle()
    {
        StartCoroutine(lightScale(5, 3.5f));
        StartCoroutine(coreLightScale(5, 50));


    }


    IEnumerator coreLightScale(float time, float size)
    {
        ParticleSystem.MainModule coreLightMain = coreLight.main;

        for (float i = 0; i < size; i += Time.deltaTime * (size / time))
        {
            coreLightMain.startSize = i;
            yield return null;
        }
        coreLightMain.startSize = size;
    }


    IEnumerator lightScale(float time, float size)
    {
        ParticleSystem.MainModule lightsMain = light.main;

        for (float i = 0; i < size; i+= Time.deltaTime * (size / time))
        {
            lightsMain.startSize = i;
            yield return null;
        }
        lightsMain.startSize = size;
    }
}