﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DealDamageTouch : MonoBehaviour
{
    public int team = 1;
    public bool selfDestructWhenDealingDamage = true;

    [SerializeField] private float damage = 1;

    [SerializeField] private UnityEvent OnDestroy;

    private Hub hub;


    private void Awake()
    {
        hub = FindObjectOfType<Hub>();
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.GetComponent<VitalSigns>())
        {
            VitalSigns other = collision.transform.GetComponent<VitalSigns>();

            if (other.team != this.team)
            {
                if (other.TakeDamage(damage))
                {
                    hub.Console.ShowMassage(collision.transform.name + " take damage '" + damage + "'");
                    if (selfDestructWhenDealingDamage)
                        Destroy();
                }
            }
        }
    }


    private void Destroy()
    {
        OnDestroy.Invoke();
        Destroy(gameObject);
    }
}