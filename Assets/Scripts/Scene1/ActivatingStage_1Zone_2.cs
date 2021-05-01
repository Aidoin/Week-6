using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatingStage_1Zone_2 : MonoBehaviour
{
    [SerializeField] private Menu menu;
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip clip1;
    [SerializeField] private AudioClip clip2;
    [SerializeField] private Light lightWorld;
    [SerializeField] private GameObject rain;
    [SerializeField] private Transform playerTransform;

    [SerializeField] [Range(0,1)]private float brightnessTheGlow;
    [SerializeField] private float rateOfChange = 1;

    private bool activated = false;


    private void OnTriggerEnter(Collider other)
    {
        if (!activated && other.attachedRigidbody.transform == playerTransform)
        {
            audio.clip = clip1;
            menu.SetVolumeMusicBacground(audio.volume + 0.2f);
            audio.Play();
            StartCoroutine(SwitchingMusicSegment());
            activated = true;
            rain.SetActive(true);

            StartCoroutine(Darkening());
        }
    }


    private IEnumerator Darkening()
    {
        for (float i = 1; i > brightnessTheGlow; i -= Time.deltaTime * rateOfChange)
        {
            RenderSettings.ambientSkyColor = Color.HSVToRGB(0, 0, i);
            lightWorld.intensity = i;

            lightWorld.shadowStrength = (i - brightnessTheGlow) / (1 - brightnessTheGlow);
            yield return null;
        }

        RenderSettings.ambientSkyColor = Color.HSVToRGB(0, 0, brightnessTheGlow);
        lightWorld.intensity = brightnessTheGlow;
    }



    private IEnumerator SwitchingMusicSegment()
    {
        yield return new WaitForSecondsRealtime(5.305f);
        audio.clip = clip2;
        audio.Play();
    }
}