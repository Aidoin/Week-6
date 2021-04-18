using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearSpawnRocket : MonoBehaviour
{

    public int Team = 1;

    [SerializeField] private GameObject rocket;
    [SerializeField] private Transform spawn;
    [SerializeField] private AudioSource shotAudio;

    [SerializeField] private Collider[] colliders;

    [SerializeField] private float damage;
    [SerializeField] private float speedRocket;


    public void Attack()
    {
        GameObject newRocket = Instantiate(rocket, spawn.position, spawn.rotation);
        DealDamageTouch dealDamageNewRocket = newRocket.GetComponent<DealDamageTouch>();

        dealDamageNewRocket.Team = Team;
        dealDamageNewRocket.Damage = damage;

        for (int i = 0; i < colliders.Length; i++)
        {
            Physics.IgnoreCollision(newRocket.GetComponent<Collider>(), colliders[i]);
        }

        shotAudio.pitch = Random.Range(0.9f, 1.1f);
        shotAudio.Play();
    }
}