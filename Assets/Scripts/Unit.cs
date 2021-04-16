using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Unit : MonoBehaviour
{
    
    [SerializeField] protected Animator animator;

    [SerializeField] protected AudioSource audioStay;

    [SerializeField] protected AudioSource audioTakeDamage;

    [SerializeField] protected AudioSource audioDeath;

    protected Rigidbody rigidbody;

    protected Hub hub;


    protected float muteSoundWhenTakingDamage = 1f;


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        hub = FindObjectOfType<Hub>();
    }


    public virtual void TakeDamage()
    {
        float volume = audioStay.volume;
        audioStay.volume = 0;
        audioTakeDamage.Play();  

        StartCoroutine(PlaySound(audioStay, muteSoundWhenTakingDamage, volume));
    }


    public virtual void Death()
    {
        audioStay.Stop();
        audioDeath.Play();
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        rigidbody.constraints = RigidbodyConstraints.None;

        animator.SetTrigger("Death");

        Component[] components = gameObject.GetComponents(typeof(Component));
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] is Collider || components[i] is Rigidbody || components[i] is Transform)
            {
                // None
            }
            else
            {
                Destroy(components[i]);
            }
        }
        Destroy(gameObject, 10);
    }


    private IEnumerator PlaySound(AudioSource audio, float timeDistance, float volumeAudio)
    {
        for (float pastTense = 0; pastTense < timeDistance; pastTense += Time.deltaTime)
        {
            yield return null;
        }
        audio.volume = volumeAudio;
    }
}