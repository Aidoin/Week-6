using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealsPack : MonoBehaviour
{

    [SerializeField] private float amountOfTreatment = 1;

    private Hub hub;
    private bool isUsed = false;


    private void Start()
    {
        hub = FindObjectOfType<Hub>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.gameObject == hub.Player)
        {
            if (!isUsed)
            {
                isUsed = true;
                other.attachedRigidbody.GetComponent<VitalSigns>().HealthRestore(amountOfTreatment);
                Destroy(gameObject);
            }
        }
    }
}