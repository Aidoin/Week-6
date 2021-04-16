using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Unit
{

    [SerializeField] private Collider[] colliders;

    [SerializeField] private float visibilityRange_float = 13;
    [SerializeField] private float timeBetweenShots = 3;

    private float timeShots = 0;


    private void FixedUpdate()
    {
        timeShots += Time.fixedDeltaTime;

        Transform playerTransform = hub.Player.transform;

        Debug.DrawRay(transform.position, (playerTransform.position - transform.position).normalized * visibilityRange_float, Color.blue);
        if (timeShots >= timeBetweenShots)
        {
            if (Vector3.Distance(playerTransform.position, transform.position) < visibilityRange_float)
            {
                timeShots = 0;
                animator.SetTrigger("Attack");
            }
        }
    }


    public override void Death()
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].isTrigger = false;
        }

        base.Death();
    }
}