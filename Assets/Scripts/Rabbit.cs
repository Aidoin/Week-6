using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Unit
{

    [SerializeField] private float visibilityRange_float = 13;
    [SerializeField] private float timeBetweenShots = 3;
    
    private float timeShots = 0;


    private void FixedUpdate()
    {
        timeShots += Time.fixedDeltaTime;

        Transform playerTransform = MyHub.Player.transform;

        Debug.DrawRay(transform.position, (playerTransform.position - transform.position).normalized * visibilityRange_float, Color.blue);
        if (timeShots >= timeBetweenShots)
        {
            if (Vector3.Distance(playerTransform.position, transform.position) < visibilityRange_float)
            {
                timeShots = 0;
                MyAnimator.SetTrigger("Attack");
            }
        }
    }
}