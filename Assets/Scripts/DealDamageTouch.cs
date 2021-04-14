using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DealDamageTouch : MonoBehaviour
{

    public float Damage = 1;
    public int Team = 1;
    public bool SelfDestructWhenDealingDamage = true;

    [SerializeField] private UnityEvent OnDestroy;


    private void OnCollisionStay(Collision collision)
    {
        Touch(collision.transform);
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody)
            Touch(other.attachedRigidbody.transform);
    }


    private void Touch(Transform otherTransform)
    {
        if (otherTransform.GetComponent<VitalSigns>())
        {
            VitalSigns other = otherTransform.GetComponent<VitalSigns>();

            if (other.team != this.Team)
            {
                other.TakeDamage(Damage);

                if (SelfDestructWhenDealingDamage)
                    Destroy();
            }
        }
    }


    private void Destroy()
    {
        OnDestroy.Invoke();
        Destroy(gameObject);
    }
}