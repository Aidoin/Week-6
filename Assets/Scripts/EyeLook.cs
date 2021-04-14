using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeLook : MonoBehaviour
{

    [SerializeField] private Transform Aim;


    void Update()
    {
        Vector3 toAim = Aim.position - transform.position; // вектор расстояния до прицела

        Vector3 toAimX = new Vector3(toAim.x, 0f,0f); // вектор с х позиции расстояния

        float angle = Vector3.SignedAngle(toAim, toAimX, Vector3.forward * Mathf.Sign(toAim.x));

        angle = Mathf.Clamp(angle,-15f,25f);

        Quaternion targetRotation = Quaternion.Euler(new Vector3(angle, 0f, 0f));

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, 1f);
    }
}