using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointNew : MonoBehaviour
{
    private Hub hub;


    private void Awake()
    {
        hub = FindObjectOfType<Hub>();
    }


    // Обновление чекпоинта
    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody && other.attachedRigidbody.gameObject == hub.Player)
        {
            other.attachedRigidbody.GetComponent<CheckpointController>().UpdateLastCheckpoint(transform.position);
        }
    }
}