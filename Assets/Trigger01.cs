using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger01 : MonoBehaviour
{
    [SerializeField] private AudioSource aud;
    [SerializeField] private AudioClip aud2;
    [SerializeField] private AudioClip aud3;

    private Hub hub;

    private bool srabotal = false;


    private void Awake()
    {
        hub = FindObjectOfType<Hub>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!srabotal && other.attachedRigidbody.gameObject == hub.Player)
        {
            aud.clip = aud2;
            aud.volume = 0.5f;
            aud.Play();
            StartCoroutine(qwer());
            srabotal = true;
        }
    }


    private IEnumerator qwer()
    {
        yield return new WaitForSeconds(5.305f);
        aud.clip = aud3;
        aud.Play();
    }
}
