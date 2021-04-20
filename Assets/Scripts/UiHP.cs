using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHP : MonoBehaviour
{

    [SerializeField] private GameObject heart;
    [SerializeField] private Transform heartConteiner;
    [SerializeField] private VitalSigns playerVitalSigns;

    public List<GameObject> heartList = new List<GameObject>();
    

    private void Start()
    {



        int numberOfHearts = Mathf.FloorToInt(playerVitalSigns.Health);

        for (int i = 0; i < numberOfHearts; i++)
        {
            heartList.Add(Instantiate(heart, heartConteiner));
        }
    }


    public void HealthChange()
    {
        int difference = 0;

        int numberOfHearts = Mathf.FloorToInt(playerVitalSigns.Health);

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