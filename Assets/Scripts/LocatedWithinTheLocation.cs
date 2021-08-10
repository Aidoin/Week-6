using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocatedWithinTheLocation : MonoBehaviour
{

    [SerializeField] private LocationsName locationsName;
    private Hub hub;


    private void Awake()
    {
        hub = FindObjectOfType<Hub>();    
    }

    // Нахождение в локации
    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.gameObject == hub.Player)
        {
            hub.LocationController.EnteringLocation(locationsName);
        }
    }

    // ВЫход из локации
    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.gameObject == hub.Player)
        {
            hub.LocationController.ExitFromLocation(locationsName);
        }
    }
}