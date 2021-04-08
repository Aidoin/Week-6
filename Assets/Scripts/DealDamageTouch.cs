using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DealDamageTouch : MonoBehaviour
{
    public int team = 1;

    [SerializeField] private float damage = 1;
    [SerializeField] private bool selfDestructWhenDealingDamage = true;

    [SerializeField] private UnityEvent OnDestroy;


    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.GetComponent<VitalSigns>())
        {
            VitalSigns other = collision.transform.GetComponent<VitalSigns>();

            if (other.team != this.team)
            {
                if (other.TakeDamage(damage))
                {
                    Debug.Log(collision.transform.name + " take damage '" + damage + "'");
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
