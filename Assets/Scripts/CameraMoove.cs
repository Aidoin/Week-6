using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoove : MonoBehaviour
{

    [SerializeField] private KeyCode zoom_KeyCode;

    [SerializeField] private Vector3 offset = Vector3.zero;

    [SerializeField] private float minDistance = 10;
    [SerializeField] private float maxDistance = 20;

    private Hub hub;

    private Vector3 sum;
    private Vector3 target;

    private float distance;
    private bool zoom = true;

    private void Awake()
    {
        hub = FindObjectOfType<Hub>();

        distance = minDistance;

        transform.parent = null;
    }


    void Update()
    {
        target = hub.Player.transform.position;

        target.y += 1f;

        if (Input.GetKeyDown(zoom_KeyCode))
        {
            if (zoom == true)
            {
                zoom = false;
                distance = minDistance;
            }
            else
            {
                zoom = true;
                distance = maxDistance;
            }
        }
        

        sum = hub.Player.transform.position + offset;
        sum.z += distance;

        transform.position = Vector3.Lerp(transform.position, sum, Time.deltaTime * 5);




        //sum = hub.Player.transform.position + offset;
        //sum = new Vector3(sum.x, sum.y, distance);








        //    if (Input.GetKeyDown(zoom_KeyCode))
        //    {
        //        if (zoom == true)
        //            zoom = false;
        //        else
        //            zoom = true;
        //    }




        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(hub.Player.transform.position, Vector3.forward), Time.deltaTime * 10);

        //if (zoom)
        //    distance = minDistance;
        //else
        //    distance = maxDistance;

        //sum = hub.Player.transform.position + offset;
        //sum.z += distance;




        // transform.position = Vector3.MoveTowards(transform.position, sum, Time.deltaTime * 5);

        //sum = hub.Player.transform.position + offset;
        //sum = new Vector3(sum.x, sum.y, Mathf.Lerp(sum.z, distance, Time.deltaTime * 10));

        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(result.x, result.y, Mathf.MoveTowards(transform.position.z, minDistance, Time.deltaTime * 40)), Time.deltaTime * 10);
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(result.x, result.y, Mathf.MoveTowards(transform.position.z, maxDistance, Time.deltaTime * 20)), Time.deltaTime * 10);

    }


    private void LateUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation((target - transform.position).normalized), Time.deltaTime * 10);
    }
}