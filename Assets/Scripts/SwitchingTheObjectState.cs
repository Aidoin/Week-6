using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SwitchingTheObjectState : MonoBehaviour
{

    [SerializeField] private float activationDistance = 20;
    [SerializeField] private bool switchingOffAtDistance = true;

    private CheckingTheObjectsStatusSwitch checkingSwitches;

    private bool isActive = true;


    private void Awake()
    {
        checkingSwitches = FindObjectOfType<CheckingTheObjectsStatusSwitch>();

        checkingSwitches.SwitchesObjects.Add(this);

        if (activationDistance < 0)
            activationDistance = activationDistance * -1;
    }


    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.grey;
        Handles.DrawWireDisc(transform.position, Vector3.forward, activationDistance);
    }


    public void CheckingDistance(Vector3 playerPosition)
    {
        if (switchingOffAtDistance)
        {
            float distance = Vector3.Distance(transform.position, playerPosition);

            if (isActive)
            {
                if (distance > activationDistance)
                    Deactivate(); 
            }
            else
            {
                if (distance < activationDistance)
                    Activate();
            }
        }
    }
    

    public void Activate()
    {
        gameObject.SetActive(true);
        isActive = true;
    }


    public void Deactivate()
    {
        gameObject.SetActive(false);
        isActive = false;
    }

    private void OnDestroy()
    {
        checkingSwitches.SwitchesObjects.Remove(this);
    }
}