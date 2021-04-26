using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SwitchingTheObjectState : MonoBehaviour
{

    public bool switchingOffAtDistance = true;

    [SerializeField] private float activationDistance = 20;

    private CheckingTheObjectsStatusSwitch checkingSwitches;

    private bool isActive = true;


    private void Awake()
    {
        checkingSwitches = FindObjectOfType<CheckingTheObjectsStatusSwitch>();

        checkingSwitches.SwitchesObjects.Add(this);

        if (activationDistance < 0)
            activationDistance = activationDistance * -1;
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


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.grey;
        Handles.DrawWireDisc(transform.position, Vector3.forward, activationDistance);
    }
#endif
}