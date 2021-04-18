using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoove : MonoBehaviour
{

    [SerializeField] private KeyCode zoom_KeyCode;

    [SerializeField] private float minDistance = 10;
    [SerializeField] private float maxDistance = 20;

    private Hub hub;

    private bool zoom = true;

    private void Awake()
    {
        hub = FindObjectOfType<Hub>();
    }


    void Update()
    {
        Transform playerTransform = hub.Player.transform;

        if (Input.GetKeyDown(zoom_KeyCode))
        {
            if (zoom == true)
                zoom = false;
            else
                zoom = true;
        }


        if (zoom)
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, Mathf.MoveTowards(transform.position.z, minDistance, Time.deltaTime * 40));
        else
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, Mathf.MoveTowards(transform.position.z, maxDistance, Time.deltaTime * 20));
    }
}