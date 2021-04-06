using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DealDamageTouch : MonoBehaviour
{
    [SerializeField] private float damage;

    [SerializeField] private UnityEvent OnDestroy;


    private void OnCollisionStay(Collision collision)
    {

        if(collision.transform.GetComponent<VitalSigns>() && !collision.transform.GetComponent<VitalSigns>().Isinvulnerability)
        {
        Debug.Log(collision.transform.name);

            collision.transform.GetComponent<VitalSigns>().TakeDamage(damage);
            Destroy();
        }
    }

    private void Destroy()
    {
        OnDestroy.Invoke();
        Destroy(gameObject);
    }
}
