using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealsPack : MonoBehaviour
{

    [SerializeField] private GameObject effect;
    [SerializeField] private float amountOfTreatment = 1;

    private Hub hub;
    private bool isUsed = false;


    private void Awake()
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

                Destroy(Instantiate(effect, transform.position, Quaternion.identity), 1);

                Destroy(gameObject);
            }
        }
    }
}