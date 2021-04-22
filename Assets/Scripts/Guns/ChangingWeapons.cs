using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct GunsEquipment
{
    public GameObject Gun;
    public bool IsPresent;
}


public class ChangingWeapons : MonoBehaviour
{

    //public KeyCode PreviousWeapon;
    //public KeyCode NextWeapon;
    
    [Header("Кнопки оружия")]
    public KeyCode Revolver;
    public KeyCode Rifle;
    public KeyCode SawedShotgun;
    public KeyCode SniperRifle;
    public KeyCode Submachine;

    public List<GunsEquipment> Weapons = new List<GunsEquipment>();

    public int CurrentWeapons => currentWeapons;
    private int currentWeapons = 0;


    private void Update()
    {
        // Смена оружия

        // Колёсико мыши
        if (Input.mouseScrollDelta.y != 0)
            ChangingWeaponsWithTheWheel(Convert.ToInt32(Input.mouseScrollDelta.y));


        // Кнгопки
        if (Input.GetKeyDown(Revolver))
            GetAWeapon(0);

        if (Input.GetKeyDown(Rifle))
            GetAWeapon(1);

        if (Input.GetKeyDown(SawedShotgun))
            GetAWeapon(2);

        if (Input.GetKeyDown(SniperRifle))
            GetAWeapon(3);

        if (Input.GetKeyDown(Submachine))
            GetAWeapon(4);
    }


    // Получение нового оружия
    public void GetAWeapon(int weaponNumber)
    {
        if (Weapons[weaponNumber].IsPresent)
        {
            HideWeapons();
            Weapons[weaponNumber].Gun.SetActive(true);
            currentWeapons = weaponNumber;
        }
    }


    private void HideWeapons()
    {
        for (int i = 0; i < Weapons.Count; i++)
        {
            Weapons[i].Gun.SetActive(false);
        }
    }


    // Смена оружия прокруткой колёсика (тут пролтстываются оружия которых нет)
    private void ChangingWeaponsWithTheWheel(int direction)
    {
        HideWeapons();

        for (int i = currentWeapons + direction; i < Weapons.Count; i += direction)
        {
            if (i < 0) // Если листается назад, за револьвер
            {
                break;
            }

            if (Weapons[i].IsPresent)
            {
                currentWeapons = i;
                break;
            }
        }

        Weapons[currentWeapons].Gun.SetActive(true);
    }
}