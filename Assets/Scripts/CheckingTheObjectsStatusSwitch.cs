using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckingTheObjectsStatusSwitch : MonoBehaviour
{
    public List<SwitchingTheObjectState> SwitchesObjects = new List<SwitchingTheObjectState>();

    [SerializeField] private Transform playerTransform; 


    private void FixedUpdate()
    {
        for (int i = 0; i < SwitchesObjects.Count; i++)
        {
            SwitchesObjects[i].CheckingDistance(playerTransform.position);
        }
    }
}