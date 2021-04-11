using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 

public class ChickenBehavior : MonoBehaviour
{

    [SerializeField] private float maxSpeed_float = 1;
    [SerializeField] private float timeToMaxSpeed_float = 1;
    [SerializeField] private float visibilityRange_float = 5;

    private Hub hub;
    private Rigidbody rigidbody;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        hub = FindObjectOfType<Hub>();
    }


    void Update()
    {
        Transform playerTransform = hub.Player.transform;

        Debug.DrawRay(transform.position, (playerTransform.position - transform.position).normalized * visibilityRange_float, Color.blue);

        if (Vector3.Distance(playerTransform.position, transform.position) < visibilityRange_float)
        {
            Vector3 toPlayer = (playerTransform.position - transform.position).normalized;
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
