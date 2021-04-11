using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Carrot : MonoBehaviour
{

    private Rigidbody rigidbody;

    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        //rigidbody.add
    }
}