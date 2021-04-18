using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Unit
{

    [SerializeField] private float timeBetweenShots = 10;

    private float timeShots = 10;


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