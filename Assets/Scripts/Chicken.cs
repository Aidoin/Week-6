using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 

public class Chicken : MonoBehaviour
{

    [SerializeField] private AudioSource stay;
    [SerializeField] private AudioSource takeDamage;
    [SerializeField] private AudioSource death;
    [SerializeField] private Animator animator;

    [SerializeField] private float maxSpeed_float = 1;
    [SerializeField] private float timeToMaxSpeed_float = 1;
    [SerializeField] private float visibilityRange_float = 5;

    private Hub hub;
    private Rigidbody rigidbody;


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        hub = FindObjectOfType<Hub>();
    }


    void FixedUpdate()
    {
        Transform playerTransform = hub.Player.transform;

        Debug.DrawRay(transform.position, (playerTransform.position - transform.position).normalized * visibilityRange_float, Color.blue);

        if (Vector3.Distance(playerTransform.position, transform.position) < visibilityRange_float)
        {
            Vector3 toPlayer = (playerTransform.position - transform.position).normalized;
            Vector3 force = rigidbody.mass * (toPlayer * maxSpeed_float - rigidbody.velocity) / timeToMaxSpeed_float;

            rigidbody.AddForce(force);
        }
        else
        {
            if (rigidbody.velocity.magnitude < 0.1f)
                rigidbody.velocity = Vector3.zero;
            else
                rigidbody.AddForce(-rigidbody.velocity.normalized * 5f);
        }
    }


    public void TakeDamage()
    {
        float volume = stay.volume;
        stay.volume = 0;
        takeDamage.Play();

        StartCoroutine(PlaySound(stay, 1f, volume));
    }


    public void Death()
    {
        stay.Stop();
        death.Play();
        rigidbody.useGravity = true;
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