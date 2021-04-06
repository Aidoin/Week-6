using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixetObjects : MonoBehaviour
{
    [SerializeField] private Transform Target;


    void Update()
    {
        transform.position = Target.position;
    }
}