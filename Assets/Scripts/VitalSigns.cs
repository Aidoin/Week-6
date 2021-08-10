using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VitalSigns : MonoBehaviour
{

    public int team = 1;
    public bool IsInvulnerability = false;

    [HideInInspector] public float Health => health;

    [SerializeField] private float health = 1;
    [SerializeField] private float maxHealth = 5;
    [SerializeField] private float armor = 0;
    [SerializeField] private float maxArmor = 5;
    [SerializeField] private float invulnerabilityTime = 2;
    [SerializeField] private bool invulnerabilityWhenTakingDamage = false;

    public UnityEvent OnTakeDamage;
    public UnityEvent OnDeath;
    [HideInInspector] public UnityEvent OnHealthRestore;
    [HideInInspector] public UnityEvent OnArmorRestore;
    [HideInInspector] public UnityEvent OnArmorEnabled;
    [HideInInspector] public UnityEvent OnIsinvulnerability;

    private Hub hub;
    private bool armorEnabled = true;


    private void Start()
    {
        hub = FindObjectOfType<Hub>();
    }


    private void OnEnable()
    {
        // Фикс того, что при пропадании моба время до окончания неуязвимости сбивается и он становится неуязвим на всегда
        IsInvulnerability = false;
    }


    public void Death()
    {
        health = 0;
        hub.Console.ShowMassage(name + " убит");
        OnDeath.Invoke();
    }


    public void SetInInvulnerability(float invulnerabilityTime)
    {
        StartCoroutine(InInvulnerability(invulnerabilityTime));
    }


    private IEnumerator InInvulnerability(float invulnerabilityTime)
    {
        IsInvulnerability = true;

        for (float time = 0; time < invulnerabilityTime; time += Time.deltaTime)
        {
            OnIsinvulnerability.Invoke();
            yield return null;
        }
        IsInvulnerability = false;
    }


    /// Методы для взаимодействия

    public bool TakeDamage(float value)
    {
        if (IsInvulnerability)
        {
            //hub.Console.ShowMassage(name + " Сейчас неуязвим");
            return false;
        }

        if (value < 0)
        {
            hub.Console.ShowMassage("Значение урона не может быть меньше нуля");
            return false;
        }

        if (armorEnabled && armor > 0)
        {
            armor -= value / 3;
            value = value / 2;
            if (armor < 0)
                armor = 0;
        }

        health -= value;
        hub.Console.ShowMassage(name + " получил '" + value + "'" + " урона");

        if (health <= 0)
        {
            health = 0;
            Death();
        }
        else if(invulnerabilityWhenTakingDamage)
        {
            StartCoroutine(InInvulnerability(invulnerabilityTime));
        }

        OnTakeDamage.Invoke();
        return true;
    }


    public bool HealthRestore(float value)
    {
        if (value < 0)
        {
            hub.Console.ShowMassage("Значение лечения не может быть меньше нуля");
            return false;
        }

        health = Mathf.Clamp(health += value, 0, maxHealth);
        OnHealthRestore.Invoke();
        return true;
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