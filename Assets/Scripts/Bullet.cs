using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [HideInInspector] public int team = 1;
    [SerializeField] private GameObject Effect;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<VitalSigns>())
        {
            if (collision.transform.GetComponent<VitalSigns>().team == this.team)
                hit();
        } else
            hit();
    }


    public void hit()
    {
        Instantiate(Effect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
