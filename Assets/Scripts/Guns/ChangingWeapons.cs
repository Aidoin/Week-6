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

    public List<GunsEquipment> Weapons = new List<GunsEquipment>();

    [SerializeField] private KeyBinding keyBinding;

    public int CurrentWeapons => currentWeaponsIndex;
    private int currentWeaponsIndex = 0;


    private void Update()
    {
        // Смена оружия

        // Колёсико мыши
        if (Input.mouseScrollDelta.y != 0)
            ChangingWeaponsWithTheWheel((int)(Input.mouseScrollDelta.y));


        // Кнгопки
        if (Input.GetKeyDown(keyBinding.Revolver))
            GetAWeapon(0);

        if (Input.GetKeyDown(keyBinding.Rifle))
            GetAWeapon(1);

        if (Input.GetKeyDown(keyBinding.SawedShotgun))
            GetAWeapon(2);

        if (Input.GetKeyDown(keyBinding.SniperRifle))
            GetAWeapon(3);

        if (Input.GetKeyDown(keyBinding.Submachine))
            GetAWeapon(4);
    }


    // Получение нового оружия
    public void GetAWeapon(int weaponNumber)
    {
        if (Weapons[weaponNumber].IsPresent)
        {
            HideWeapons();
            Weapons[weaponNumber].Gun.SetActive(true);
            currentWeaponsIndex = weaponNumber;
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
        for (int index = currentWeaponsIndex + direction; index < Weapons.Count; index += direction)
        {
            if (index < 0) // Если листается назад, за револьвер
            {
                break;
            }

            if (Weapons[index].IsPresent)
            {
                GetAWeapon(index);
                break;
            }
        }
    }
}