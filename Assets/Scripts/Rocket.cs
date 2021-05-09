using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Projectile))]

public class Rocket : MonoBehaviour
{

    public float SpeedMoove = 5;
    public float SpeedRotation = 5;

    [SerializeField] private Transform model;

    private new Rigidbody rigidbody;
    private Hub hub;

    private Vector3 toPlayer;
    private Vector3 verticalRtation; // Поворот вниз ногами


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        hub = FindObjectOfType<Hub>();
    }


    private void FixedUpdate()
    {
        toPlayer = (hub.Player.transform.position - transform.position).normalized;

        rigidbody.velocity = transform.forward * (SpeedMoove);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(toPlayer, -Vector3.forward), SpeedRotation);

        
        if (model.forward.x < 0)
            verticalRtation = new Vector3(0, 0, 180);
        else
            verticalRtation = Vector3.zero;

        model.localRotation = Quaternion.RotateTowards(model.localRotation, Quaternion.Euler(verticalRtation), 10);
    }
}