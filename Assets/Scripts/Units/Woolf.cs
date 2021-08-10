using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woolf : Unit
{


    [SerializeField] private GameObject EffectSmoge;

    [SerializeField] private AudioClip audioBeginningAttack;
    [SerializeField] private AudioClip audioContinuingAttack;

    [SerializeField] float maxSpeed_float = 15;
    [SerializeField] float timeToMaxSpeed_float = 2;
    [SerializeField] float rotationSpeed_float = 20;
    [SerializeField] private float timeBetweenAttacks = 5;

    private AudioClip clipStay;
    private SwitchingTheObjectState switchingState;

    private Vector3 toTarget;
    private Vector3 targetEuler;

    private float audioVolumeStayStart;
    private bool isAttacking = false;
    private bool defaultSwitchingOffAtDistance;


    private new void Awake()
    {
        base.Awake();

        switchingState = GetComponent<SwitchingTheObjectState>();
        defaultSwitchingOffAtDistance = switchingState.switchingOffAtDistance;
    }


    private void Start()
    {
        clipStay = audioStay.clip;
        audioVolumeStayStart = audioStay.volume;
    }


    private new void FixedUpdate()
    {
        if (alive == false)
            return;

        base.FixedUpdate();

        if (!isAttacking && distanceToPlayer < visibilityRange_float)
        {
            if (FindSide() == Side.Left)
                targetEuler.y = -10;
            else
                targetEuler.y = -170;


            toTarget = (playerTransform.position - transform.position).normalized;
            StartCoroutine(Attacking());
            isAttacking = true;
            switchingState.switchingOffAtDistance = false;
        }
    }


    private void SetAudioClip(AudioClip newClip, float soundVolume)
    {
        audioStay.Stop();
        audioStay.clip = newClip;
        audioStay.volume = soundVolume;
        audioStay.Play();
    }


    // Отдаляем в какой стороне игрок от свинки
    private Side FindSide()
    {
        if (playerTransform.position.x > transform.position.x)
            return Side.Left;
        else
            return Side.Right;
    }


    private IEnumerator Attacking()
    {
        Side targetSide = FindSide();

        // Поворот свинки в направлении игрока
        while (transform.rotation != Quaternion.Euler(targetEuler))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetEuler), rotationSpeed_float);

            yield return new WaitForFixedUpdate();
        }

        // Начало атаки (Анимация агра)
        SetAudioClip(audioBeginningAttack, audioStay.volume);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(2f);


        // Атака 
        SetAudioClip(audioContinuingAttack, 1);

        // Если игрок за анимацию агра перешёл на другую сторону свинки - направление обновляется
        Side side = FindSide();
        if (side != targetSide)
        {
            if (side == Side.Left)
                targetEuler.y = -10;
            else
                targetEuler.y = -170;

            toTarget = (playerTransform.position - transform.position).normalized;

            // Поворот свинки в направлении игрока
            while (transform.rotation != Quaternion.Euler(targetEuler))
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetEuler), rotationSpeed_float);

                yield return new WaitForFixedUpdate();
            }
        }

        // Свинка бежит к игроку пока не выровняется с ним по координате X
        EffectSmoge.SetActive(true);

        while (Mathf.Abs(transform.position.x - playerTransform.position.x) > 3)
        {
            Vector3 force = rigidbody.mass * (new Vector3(toTarget.x, 0, 0) * maxSpeed_float - rigidbody.velocity) / timeToMaxSpeed_float;

            rigidbody.AddForce(force);

            yield return new WaitForFixedUpdate();
        }

        animator.SetTrigger("StopAttack");

        while (rigidbody.velocity.magnitude > 2)
        {
            rigidbody.velocity -= new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0) * 0.02f; // Замедление

            yield return new WaitForFixedUpdate();
        }

        SetAudioClip(clipStay, audioVolumeStayStart);
        animator.SetTrigger("Stop");
        EffectSmoge.SetActive(false);

        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
        switchingState.switchingOffAtDistance = defaultSwitchingOffAtDistance;
    }
}
