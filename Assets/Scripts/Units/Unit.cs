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
    protected float muteSoundWhenTakingDamage = 1f;

    protected bool alive = true;
    //protected bool IsPlayAudioTakeDamage = false;


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
    }


    protected void FixedUpdate()
    {
        if (alive == false) return;

        toPlayer = (playerTransform.position - transform.position).normalized;
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        Debug.DrawRay(transform.position, toPlayer * visibilityRange_float, Color.blue);
        
        if(boolTurnToPlayerWhenDetected)
        { Debug.Log("ertgwer");
            if (playerTransform.position.x > transform.position.x)
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
           alive = false;
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


        //for (int i = 0; i < ComponentsToRemoveAtDeath.Length; i++)
        //{
        //    if (ComponentsToRemoveAtDeath[i] is SwitchingTheObjectState)
        //    {
        //        SwitchingTheObjectState switcer = (SwitchingTheObjectState)ComponentsToRemoveAtDeath[i];
        //        switcer.switchingOffAtDistance = false;
        //    }
        //    else
        //    {
        //        Destroy(ComponentsToRemoveAtDeath[i]);
        //    }
        //}

        
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