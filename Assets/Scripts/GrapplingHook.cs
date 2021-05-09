using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{

    public new Rigidbody rigidbody;


    [SerializeField] private GrapplingHookGun gum;

    [SerializeField] private GameObject player;
    [SerializeField] private Collider myCollider;
    [SerializeField] private Collider[] playerColliders;



    private FixedJoint fixedJoint;


    public bool qwe;


    private void Start()
    {
        for (int i = 0; i < playerColliders.Length; i++)
        {
            Physics.IgnoreCollision(myCollider, playerColliders[i]);
        }
    }


    private void Update()
    {
        if(qwe)
        {
            qwe = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (fixedJoint == null)
        {
            fixedJoint = gameObject.AddComponent<FixedJoint>();

            if (collision.rigidbody)
            {
                fixedJoint.connectedBody = collision.rigidbody;
            }

            gum.CreateSpringJoint();
        }
    }

    public void UnHook()
    {
        Destroy(fixedJoint);
    }
}