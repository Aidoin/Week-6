using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Projectile : MonoBehaviour
{

    [HideInInspector] public int team = 1;
    [SerializeField] private GameObject effect;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<VitalSigns>())
        {
            if (collision.transform.GetComponent<VitalSigns>().team == this.team)
                hit();
        }
        else
            hit();
    }


    public void hit()
    {
        Destroy(Instantiate(effect, transform.position, Quaternion.identity), 1);

        Destroy(gameObject);
    }
}