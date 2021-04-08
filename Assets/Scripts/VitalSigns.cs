﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VitalSigns : MonoBehaviour
{
    public int team = 1;

    [SerializeField] private float health = 1;
    [SerializeField] private float maxHealth = 5;
    [SerializeField] private float armor = 0;
    [SerializeField] private float maxArmor = 5;
    [SerializeField] private float invulnerabilityTime = 2;

    [Header("Events:")]
    [SerializeField] private UnityEvent OnTakeDamage;
    [SerializeField] private UnityEvent OnHealthRestore;
    [SerializeField] private UnityEvent OnArmorRestore;
    [SerializeField] private UnityEvent OnArmorEnabled;
    [SerializeField] private UnityEvent OnDeath;

    public bool Isinvulnerability { get; private set; } = false;

    private bool armorEnabled = true;


    public void Death()
    {
        OnDeath.Invoke();
    }


    private IEnumerator Invulnerability()
    {
        for (float time = 0; time < invulnerabilityTime; time += Time.deltaTime)
        {
            yield return null;
        }
        Isinvulnerability = false;
    }

    /// Методы для взаимодействия

    public bool TakeDamage(float value)
    {
        if (Isinvulnerability)
            return false;

        if (value < 0){
            Debug.LogError("Значение урона не может быть меньше нуля");
            return false;
        }
        else
        {
            if (armor > 0 && armorEnabled)
            {
                armor -= value / 3;
                value = value / 2;
                if (armor < 0)
                    armor = 0;
            }

            health -= value;
            if (health < 0)
            {
                health = 0;
                Death();
            }

            Isinvulnerability = true;
            StartCoroutine(Invulnerability());
            OnTakeDamage.Invoke();
            return true;
        }
    }

    public void HealthRestore(float value)
    {
        health = Mathf.Clamp(health += value, 0, maxHealth);
        OnHealthRestore.Invoke();
    }

    public void ArmorRestore(float value)
    {
        armor = Mathf.Clamp(armor += value, 0, maxArmor);
        OnArmorRestore.Invoke();
    }

    public void ArmorEnabled(bool value)
    {
        armorEnabled = value;
        OnArmorEnabled.Invoke();
    }
}
