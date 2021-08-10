using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoove : MonoBehaviour
{
    public float CameraMovementSpeed = 10;

    [SerializeField] private Transform playerTransform;

    [SerializeField] private Vector3 offset = Vector3.zero;

    [SerializeField] private float angleRotation;
    [SerializeField] private float minDistance = 10;
    [SerializeField] private float maxDistance = 20;


    private Vector3 cameraOffsetPosition;

    private float distance;


    private void Awake()
    {
        distance = minDistance;

        transform.parent = null;
    }


    private void Update()
    {
        cameraOffsetPosition = playerTransform.position + offset;
        cameraOffsetPosition.z -= distance;

        transform.position = Vector3.Lerp(transform.position, cameraOffsetPosition, Time.deltaTime * CameraMovementSpeed);

        float mouseX = 0.5f + (Input.mousePosition.x - (Screen.width / 2)) / Screen.width *2;

        float angle = Mathf.Lerp(-angleRotation, angleRotation, mouseX);

        transform.localEulerAngles = new Vector3(0f, angle, 0f);
    }

    public void DistanceMax()
    {
        distance = maxDistance;
    }

    public void DistanceMin()
    {
        distance = minDistance;
    }
}