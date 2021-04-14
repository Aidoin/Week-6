using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UiHP : MonoBehaviour
{

    [SerializeField] private GameObject heart;
    [SerializeField] private Transform heartConteiner;

    public List<GameObject> heartList = new List<GameObject>();

    private Hub hub;


    private void Awake()
    {
        hub = FindObjectOfType<Hub>();    
    }


    private void Start()
    {
        int numberOfHearts = Convert.ToInt32(Mathf.Floor(hub.Player.GetComponent<VitalSigns>().Health)); ////////////////////

        for (int i = 0; i < numberOfHearts; i++)
        {
            heartList.Add(Instantiate(heart, heartConteiner));
        }
    }


    public void HealthChange()
    {
        int difference = 0;

        int numberOfHearts = Convert.ToInt32(Mathf.Floor(hub.Player.GetComponent<VitalSigns>().Health)); /////////////////////

        if (heartList.Count < numberOfHearts)
        {
            difference = numberOfHearts - heartList.Count;

            for (int i = 0; i < difference; i++)
            {
                heartList.Add(Instantiate(heart, heartConteiner));
            } 
        }
        else if (heartList.Count > numberOfHearts)
        {
            difference = heartList.Count - numberOfHearts;

            for (int i = 0; i < difference; i++)
            {
                Destroy(heartList[heartList.Count - 1]);
                heartList.RemoveAt(heartList.Count - 1);
            }
        }
    }
}