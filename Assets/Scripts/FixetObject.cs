using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixetObject : MonoBehaviour
{
    [SerializeField] private Transform Target;


    void Update()
    {
        transform.position = Target.position;
    }
}