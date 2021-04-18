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

    [SerializeField] protected float visibilityRange_float = 10;

    [SerializeField] protected bool boolturnToPlayerWhenDetected;
    [SerializeField] protected bool isTrigger;

    [SerializeField] protected Collider[] colliders;

    protected Rigidbody rigidbody;

    protected Hub hub;

    protected Vector3 toPlayer;
    protected Vector3 targetEulerToPlayer;

    protected float distanceToPlayer;

    protected float muteSoundWhenTakingDamage = 1f;


    protected void Awake()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        rigidbody = GetComponent<Rigidbody>();

        hub = FindObjectOfType<Hub>();
        if(isTrigger)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].isTrigger = true;
                rigidbody.isKinematic = true;
            }
        }
    }


    protected void FixedUpdate()
    {
        toPlayer = (hub.Player.transform.position - transform.position).normalized;
        distanceToPlayer = Vector3.Distance(transform.position, hub.Player.transform.position);

        Debug.DrawRay(transform.position, toPlayer * visibilityRange_float, Color.blue);

        if(boolturnToPlayerWhenDetected)
        {
            if (hub.Player.transform.position.x > transform.position.x)
            {
                targetEulerToPlayer.y = -10;
            }
            else
            {
                targetEulerToPlayer.y = -170;
            }
            StartCoroutine(TurnToPlayer());
        }



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
        if (isTrigger)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].isTrigger = false;
                rigidbody.isKinematic = false;
            }
        }

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


    private IEnumerator TurnToPlayer()
    {
        while (transform.rotation != Quaternion.Euler(targetEulerToPlayer))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetEulerToPlayer), 1.5f);

            yield return new WaitForFixedUpdate();
        }
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