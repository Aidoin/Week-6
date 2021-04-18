using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pig : Unit
{

    [SerializeField] private AudioClip audioBeginningAttack;
    [SerializeField] private AudioClip audioContinuingAttack;

    [SerializeField] float maxSpeed_float = 15;
    [SerializeField] float timeToMaxSpeed_float = 2;
    [SerializeField] float rotationSpeed_float = 20;
    [SerializeField] private float timeBetweenAttacks = 5;

    private AudioClip clipStay;

    private Vector3 targetAttack;
    private Vector3 toTarget;
    private Vector3 targetEuler;

    private float audioVolumeStayStart;
    private bool isAttacking = false;

    


    private void Start()
    {
        clipStay = audioStay.clip;
        audioVolumeStayStart = audioStay.volume;
    }


    private new void FixedUpdate()
    {
        base.FixedUpdate();

        if (!isAttacking && distanceToPlayer < visibilityRange_float)
        {
            targetAttack = hub.Player.transform.position;

            // Отдаляем таргет чтобы свинка пробегала дальше чем была позиция игрока и находим в какую сторону должна смотреть свинка
            if (targetAttack.x > transform.position.x)
            {
                targetAttack.x += 10;
                targetEuler.y = -10;
            }
            else
            {
                targetAttack.x -= 10;
                targetEuler.y = -170;
            }

            toTarget = (hub.Player.transform.position - transform.position).normalized;
            StartCoroutine(Attacking());
            isAttacking = true;
        }
    }


    private void ChangingTheSound(AudioClip newClip, float soundVolume)
    {
        audioStay.Stop();
        audioStay.clip = newClip;
        audioStay.volume = soundVolume;
        audioStay.Play();
    }


    private IEnumerator Attacking()
    {
        while (transform.rotation != Quaternion.Euler(targetEuler))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetEuler), rotationSpeed_float);
            
            yield return new WaitForFixedUpdate();
        }


        ChangingTheSound(audioBeginningAttack, audioStay.volume);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(2f);


        ChangingTheSound(audioContinuingAttack, 1);

        while (Vector3.Distance(transform.position, targetAttack) > 3)
        {
            Vector3 force = rigidbody.mass * (new Vector3(toTarget.x, 0, 0) * maxSpeed_float - rigidbody.velocity) / timeToMaxSpeed_float;

            rigidbody.AddForce(force);

            yield return new WaitForFixedUpdate();
        }


        animator.SetTrigger("StopAttack");

        while (rigidbody.velocity.magnitude > 2)
        {
            rigidbody.velocity -= new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0) * 0.01f; // Замедление

            yield return new WaitForFixedUpdate();
        }

        ChangingTheSound(clipStay, audioVolumeStayStart);
        animator.SetTrigger("Stop");


        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }
}