using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 

public class Chicken : Unit
{

    [SerializeField] private float maxSpeed_float = 1;
    [SerializeField] private float timeToMaxSpeed_float = 1;
    [SerializeField] private float visibilityRange_float = 10;

   
    void FixedUpdate()
    {
        Transform playerTransform = MyHub.Player.transform;

        Debug.DrawRay(transform.position, (playerTransform.position - transform.position).normalized * visibilityRange_float, Color.blue);

        if (Vector3.Distance(playerTransform.position, transform.position) < visibilityRange_float)
        {
            Vector3 toPlayer = (playerTransform.position - transform.position).normalized;
            Vector3 force = MyRigidbody.mass * (toPlayer * maxSpeed_float - MyRigidbody.velocity) / timeToMaxSpeed_float;

            MyRigidbody.AddForce(force);
        }
        else
        {
            if (MyRigidbody.velocity.magnitude < 0.1f)
                MyRigidbody.velocity = Vector3.zero;
            else
                MyRigidbody.AddForce(-MyRigidbody.velocity.normalized * 5f);
        }
    }
}