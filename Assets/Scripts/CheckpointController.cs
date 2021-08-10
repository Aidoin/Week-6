using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{

    private Vector3 checkpointPosition;

    public void UpdateLastCheckpoint(Vector3 position)
    {
        checkpointPosition = position;
    }

    public void BackToCheckpoint()
    {
        transform.position = checkpointPosition;
    }
}