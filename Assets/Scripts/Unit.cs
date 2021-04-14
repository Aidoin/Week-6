using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Unit : MonoBehaviour
{

    [HideInInspector] public Animator MyAnimator => animator;
    [HideInInspector] public Rigidbody MyRigidbody => rigidbody;
    [HideInInspector] public Hub MyHub => hub;


    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource stay;
    [SerializeField] private AudioSource takeDamage;
    [SerializeField] private AudioSource death;

    private Rigidbody rigidbody;
    private Hub hub;
    private float muteSoundWhenTakingDamage = 1f;


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        hub = FindObjectOfType<Hub>();
    }


    public void TakeDamage()
    {
        float volume = stay.volume;
        stay.volume = 0;
        takeDamage.Play();

        StartCoroutine(PlaySound(stay, muteSoundWhenTakingDamage, volume));
    }


    public void Death()
    {
        stay.Stop();
        death.Play();
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        rigidbody.constraints = RigidbodyConstraints.None;

        animator.SetTrigger("Death");

        Component[] components = gameObject.GetComponents(typeof(Component));
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] is SphereCollider || components[i] is Rigidbody || components[i] is Transform)
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