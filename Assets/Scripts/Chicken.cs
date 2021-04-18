using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 

public class Chicken : Unit
{

    [SerializeField] private float maxSpeed_float = 1;
    [SerializeField] private float timeToMaxSpeed_float = 1;


    private new void FixedUpdate()
    {
        base.FixedUpdate();

        if (distanceToPlayer < visibilityRange_float)
        {
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
}