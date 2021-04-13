using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitSpawnCarrot : MonoBehaviour
{
    public int Team = 1;

    [SerializeField] private GameObject carrot;
    [SerializeField] private Transform spawn;
    [SerializeField] private AudioSource audio;

    [SerializeField] private Collider[] colliders;

    [SerializeField] private float liveTime;
    [SerializeField] private float speedCarrot;
    [SerializeField] private float rotationSpeed;

    private Hub hub;
    private Vector3 ToPlayer;


    private void Awake()
    {
        hub = FindObjectOfType<Hub>();
    }


    public void Attack()
    {
        ToPlayer = (hub.Player.transform.position - spawn.position).normalized;

        GameObject newCarrot = Instantiate(carrot, spawn.position, spawn.rotation);

        Rigidbody rigidbodyNewCarrot = newCarrot.GetComponent<Rigidbody>();

        newCarrot.GetComponent<DealDamageTouch>().team = Team;
        rigidbodyNewCarrot.maxAngularVelocity = Mathf.Infinity;
        rigidbodyNewCarrot.velocity = ToPlayer * speedCarrot;
        rigidbodyNewCarrot.AddTorque(Vector3.back * rotationSpeed, ForceMode.VelocityChange);

        for (int i = 0; i < colliders.Length; i++)
        {
            Physics.IgnoreCollision(newCarrot.GetComponent<CapsuleCollider>(), colliders[i]);
        }

        Destroy(newCarrot, liveTime);

        audio.pitch = Random.Range(0.9f, 1.1f);
        audio.Play();
    }
}