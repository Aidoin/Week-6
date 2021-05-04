using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedObjectPosition : MonoBehaviour
{

    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offsetPosition = Vector3.zero;

    [SerializeField] private bool position = false;
    [SerializeField] private bool rotation = false;
    [SerializeField] private bool localScale = false;



    void Update()
    {
        if (position)
        {
            transform.position = target.position + offsetPosition;
        }
        if (rotation)
        {
            transform.rotation = target.rotation;
        }
        if (localScale)
        {
            transform.localScale = target.localScale;
        }
    }
}