using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelSpawnAcorns : MonoBehaviour
{

    public int Team = 1;

    [SerializeField] private GameObject acorn;
    [SerializeField] private Transform[] spawns;
    [SerializeField] private AudioSource shotAudio;

    [SerializeField] private Collider[] colliders;

    [SerializeField] private float damage;
    [SerializeField] private float liveTime;
    [SerializeField] private float speedAcorns;


    public void Attack()
    {
            List<Collider> listCollider = new List<Collider>();

        for (int i = 0; i < spawns.Length; i++)
        {

            Vector3 random;
            random.x = Random.Range(0, 50);
            random.y = Random.Range(0, 50);
            random.z = Random.Range(0, 50);


            GameObject newAcorn = Instantiate(acorn, spawns[i].position, spawns[i].rotation);

            Rigidbody rigidbodyNewAcorn = newAcorn.GetComponent<Rigidbody>();
            DealDamageTouch dealDamageNewAcorn = newAcorn.GetComponent<DealDamageTouch>();
            Collider colliderNewAcorn = newAcorn.GetComponent<Collider>();

            dealDamageNewAcorn.Team = Team;
            dealDamageNewAcorn.Damage = damage;

            rigidbodyNewAcorn.velocity = spawns[i].forward * speedAcorns * Random.Range(0.8f, 1.5f);
            rigidbodyNewAcorn.maxAngularVelocity = Mathf.Infinity;
            rigidbodyNewAcorn.AddTorque(random, ForceMode.VelocityChange);


            for (int j = 0; j < colliders.Length; j++)
            {
                Physics.IgnoreCollision(colliderNewAcorn, colliders[j]);
            }

            listCollider.Add(colliderNewAcorn);
            Destroy(newAcorn, liveTime);
        }


        for (int i = 0; i < listCollider.Count; i++)
        {
            for (int j = i + 1; j < listCollider.Count; j++)
            {
                Physics.IgnoreCollision(listCollider[i], listCollider[j]);
            }
        }

        shotAudio.pitch = Random.Range(0.9f, 1.1f);
        shotAudio.Play();
    }
}