using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{

    [SerializeField] private RabbitSpawnCarrot rabbitSpawnCarrot;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform spawnspawnCenter;

    [SerializeField] private AudioSource stay;
    [SerializeField] private AudioSource takeDamage;
    [SerializeField] private AudioSource death;

    private Hub hub;
    private Rigidbody rigidbody;

    private Vector3 ToPlayer;

    public bool qwe = false;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        hub = FindObjectOfType<Hub>();
    }


    private void FixedUpdate()
    {
        if (qwe)
        {
            animator.SetTrigger("Attack");
            qwe = false;
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