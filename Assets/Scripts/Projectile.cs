using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Projectile : MonoBehaviour
{

    [HideInInspector] public int team = 1;

    [SerializeField] private GameObject effectOnDestroyPrefab;
    [SerializeField] private GameObject audioOnDestroy;
    [SerializeField] private bool passThroughAllies = false;

    private bool destroy = true;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<VitalSigns>())
        {
            if (!passThroughAllies && collision.transform.GetComponent<VitalSigns>().team == this.team)
                hit();
        }
        else
            hit();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
        {
            if (other.attachedRigidbody.GetComponent<VitalSigns>())
            {
                if (!passThroughAllies && other.attachedRigidbody.GetComponent<VitalSigns>().team == this.team)
                    hit();
            }
            else
                hit();
        }
    }


    public void hit()
    {
        if (destroy)
        {
            if (effectOnDestroyPrefab)
                Destroy(Instantiate(effectOnDestroyPrefab, transform.position, Quaternion.identity), 2);

            if (audioOnDestroy)
            {
                audioOnDestroy.transform.parent = null;
                audioOnDestroy.GetComponent<AudioSource>().Play();
                Destroy(audioOnDestroy, 2);
            }

            destroy = false;
            Destroy(gameObject);
        }
    }
}