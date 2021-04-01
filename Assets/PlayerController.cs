using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public KeyCode MooveRight_KeyCode;
    public KeyCode MooveLeft_KeyCode;
    public KeyCode MooveSquat_KeyCode;
    public KeyCode MooveJump_KeyCode;

    public float speedMoove_float;

    private float moovrR_float = 0, mooveL_float = 0, jump_float;
    private Rigidbody rigidbody;
    private float MooveHorizontal_float = 0;
    private bool isGraundet_bool;
    private bool jumped_bool = false;
    private float timerJump_float = 0;
    private Vector3 groundNormal;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        timerJump_float += Time.deltaTime;

        jump_float = 20;
        if (Input.GetKey(MooveJump_KeyCode))
            if (isGraundet_bool)
                jumped_bool = true;

        if (Input.GetKeyUp(MooveJump_KeyCode))
            jumped_bool = false;


        Debug.DrawRay(transform.position, groundNormal * 50, Color.red);

    }


    private void FixedUpdate()
    {
        MooveHorizontal_float = 0;

        if (Input.GetKey(MooveRight_KeyCode))
            MooveHorizontal_float += 1;
        
        if (Input.GetKey(MooveLeft_KeyCode))
            MooveHorizontal_float -= 1;
        
        if(!Input.GetKey(MooveLeft_KeyCode) && !Input.GetKey(MooveRight_KeyCode))
             MooveHorizontal_float = 0;




        if (isGraundet_bool && jumped_bool && timerJump_float > 0.5f)
        {
            timerJump_float = 0;
            jumped_bool = false;
            Debug.Log("jump");
        //rigidbody.AddForce(0, -rigidbody.velocity.y, 0, ForceMode.VelocityChange);
        //    rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.y);
            rigidbody.AddForce(0, jump_float - rigidbody.velocity.y * 1.5f, 0, ForceMode.VelocityChange);
        }





        //rigidbody.AddForce(0, 0, (MooveHorizontalRL_Vector2.x + -MooveHorizontalRL_Vector2.y) * speedMoove_float, ForceMode.VelocityChange);
        rigidbody.AddForce(groundNormal * MooveHorizontal_float * speedMoove_float, ForceMode.VelocityChange);


        if (isGraundet_bool && !jumped_bool)
        {
            //rigidbody.AddForce(0, -rigidbody.velocity.y * 0.5f, -rigidbody.velocity.z * 0.5f, ForceMode.VelocityChange);

            rigidbody.velocity -= new Vector3(0, rigidbody.velocity.y * 0.2f, rigidbody.velocity.z * 0.2f); // Замедление при движении по земле

            //rigidbody.velocity -= new Vector3(0, 0, rigidbody.velocity.z / 3); // Замедление при движении по земле
        }
        else
        {
            Debug.Log("apaba");
            //rigidbody.AddForce(0, -0.5f, -rigidbody.velocity.z * 0.5f, ForceMode.VelocityChange);
            rigidbody.velocity -= new Vector3(0, 0.5f, rigidbody.velocity.z / 3); // Замедление при движении в воздухе
        }

    }


    private void OnCollisionStay(Collision collision)
    {
        

        if (Vector3.Angle(Vector3.up, collision.contacts[0].normal) < 45)
        {
            groundNormal = Vector3.Cross(collision.contacts[0].normal, Vector3.left);

            isGraundet_bool = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        groundNormal = Vector3.forward;
        isGraundet_bool = false;
    }

}
