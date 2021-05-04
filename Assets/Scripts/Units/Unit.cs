using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(VitalSigns))]
[RequireComponent(typeof(SwitchingTheObjectState))]

public class Unit : MonoBehaviour
{
    
    [SerializeField] protected Animator animator;
    [SerializeField] protected AudioSource audioStay;
    [SerializeField] protected AudioSource audioTakeDamage;
    [SerializeField] protected AudioSource audioDeath;
    [SerializeField] protected GameObject effectDestroy;


    [SerializeField] protected float visibilityRange_float = 10;

    [SerializeField] protected bool boolTurnToPlayerWhenDetected;
    [SerializeField] protected bool isTrigger;

    [SerializeField] protected Collider[] colliders;

    protected Rigidbody rigidbody;
    protected VitalSigns vitalSigns;
    protected Transform playerTransform;

    protected Vector3 toPlayer;
    protected Vector3 targetEulerToPlayer;

    protected float distanceToPlayer;
    protected float turnLeftAngle = -10;
    protected float turnRightAngle = -170;

    protected float muteSoundWhenTakingDamage = 0.5f;
    protected bool IsPlayAudioTakeDamage = false;

    protected bool alive = true;


    protected void Awake()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        rigidbody = GetComponent<Rigidbody>();
        vitalSigns = GetComponent<VitalSigns>();

        playerTransform = FindObjectOfType<PlayerController>().transform;

        if(isTrigger)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].isTrigger = true;
                rigidbody.isKinematic = true;
            }
        }
        
        vitalSigns.OnTakeDamage.AddListener(TakeDamage);
        vitalSigns.OnDeath.AddListener(Death);

        audioStay.pitch = Random.Range(0.8f, 1.2f);
        audioTakeDamage.pitch = Random.Range(0.8f, 1.2f);
        audioDeath.pitch = Random.Range(0.8f, 1.2f);
    }


    protected void FixedUpdate()
    {
        if (alive == false) return;

        toPlayer = (playerTransform.position - transform.position).normalized;
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        Debug.DrawRay(transform.position, toPlayer * visibilityRange_float, Color.blue);
        
        if(boolTurnToPlayerWhenDetected)
        {
            if (playerTransform.position.x > transform.position.x)
            {
                targetEulerToPlayer.y = turnLeftAngle;
            }
            else
            {
                targetEulerToPlayer.y = turnRightAngle;
            }
            StartCoroutine(TurnToPlayer());
        }
    }


    public virtual void TakeDamage()
    {
        if (IsPlayAudioTakeDamage)
            return; 

        float volume = audioStay.volume;
        audioStay.volume = 0;
        audioTakeDamage.Play();


        StartCoroutine(PlaySound(audioStay, muteSoundWhenTakingDamage, volume));
    }


    public virtual void Death()
    {
        alive = false;
        StopAllCoroutines();

        vitalSigns.IsInvulnerability = true;

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
            if (components[i] is Collider || components[i] is Rigidbody || components[i] is Transform || components[i] is Unit || components[i] is VitalSigns)
            {
                // None
            }
            else
            {
                if (components[i] is SwitchingTheObjectState switcer)
                {
                    switcer.switchingOffAtDistance = false;
                }
                else
                {
                    Destroy(components[i]);
                }
            }
        }

        StartCoroutine(Destroy(3));
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
        IsPlayAudioTakeDamage = true;

        for (float pastTense = 0; pastTense < timeDistance; pastTense += Time.deltaTime)
        {
            yield return null;
        }
        audio.volume = volumeAudio;

        IsPlayAudioTakeDamage = false;
    }


    private IEnumerator Destroy(float time)
    {
        yield return new WaitForSeconds(time);

        effectDestroy.SetActive(true);
        effectDestroy.transform.parent = transform.parent;
        Destroy(gameObject);
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.forward, visibilityRange_float);
    }
#endif
}