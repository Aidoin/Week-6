using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LocationsName
{
    Start, Dungeon, DungeonEnd, Mountain, RearCastle, Desert, Tower, Market, City, Park
}


[Serializable] public struct Locations
{
    public LocationsName Name;
    public bool Active;
    public GameObject Location;
}


public class LocationController : MonoBehaviour
{
    public Locations[] Locations = new Locations[10];


    private void Start()
    {
        for (int i = 0; i < Locations.Length; i++)
        {
            Locations[i].Active = false;
        }
    }


    private void FixedUpdate()
    {
        UpdatingLocationActivity();
    }


    public void EnteringLocation(LocationsName locationsName)
    {
        for (int i = 0; i < Locations.Length; i++)
        {
            if(Locations[i].Name == locationsName)
            {
                Locations[i].Active = true;
            }
        }
    }

    public void ExitFromLocation(LocationsName locationsName)
    {
        for (int i = 0; i < Locations.Length; i++)
        {
            if (Locations[i].Name == locationsName)
            {
                Locations[i].Active = false;
            }
        }
    }

    // Обновление видимости локации
    private void UpdatingLocationActivity()
    {
        for (int i = 0; i < Locations.Length; i++)
        {
            Locations[i].Location.SetActive(Locations[i].Active);
        }
    }
}