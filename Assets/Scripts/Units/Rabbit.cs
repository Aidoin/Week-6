using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Unit
{

    [SerializeField] private float timeBetweenShots = 3;

    private float timeShots = 0;


    private new void FixedUpdate()
    {
        base.FixedUpdate();

        timeShots += Time.fixedDeltaTime;

        if (timeShots >= timeBetweenShots)
        {
            if (distanceToPlayer < visibilityRange_float)
            {
                timeShots = 0;
                animator.SetTrigger("Attack");
            }
        }
    }
}