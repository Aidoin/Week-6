using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{

    [Header("Aim")]
    [SerializeField] private Transform Aim;
    [SerializeField] private Camera PlayerCamera;


    void LateUpdate()
    {
        Ray ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(-Vector3.forward, Vector3.zero);

        float distance;
        plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance);

        Aim.position = point;


        Vector3 toAim = Aim.position - transform.position;
        
        transform.rotation = Quaternion.LookRotation(toAim);

        Debug.DrawRay(transform.position, toAim * 20, Color.cyan);
    }
}