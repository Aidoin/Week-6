using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woter : MonoBehaviour
{
    private Hub hub;


    private void Awake()
    {
        hub = FindObjectOfType<Hub>();
    }


    // Попадание в воду
    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.gameObject == hub.Player)
        {
            other.attachedRigidbody.GetComponent<VitalSigns>().Death();
        }
        else if (other.attachedRigidbody && other.attachedRigidbody.GetComponent<Unit>())
        {
            other.attachedRigidbody.GetComponent<VitalSigns>().Death();
        }
    }
}