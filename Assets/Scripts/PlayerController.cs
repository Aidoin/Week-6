using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform Body;
    [SerializeField] private Transform Eyes;
    [SerializeField] private Transform Aim;

    [SerializeField] private KeyCode MooveRight_KeyCode;
    [SerializeField] private KeyCode MooveLeft_KeyCode;
    [SerializeField] private KeyCode MooveSquat_KeyCode;
    [SerializeField] private KeyCode MooveJump_KeyCode;

    public float qwe;


    private Rigidbody rigidbody;

    private Vector3 groundNormal; // Вектор движения персонажа относительно его положения

    private float jump_float = 15; // Сила прыжка
    private float speedMoove_float = 2; // Скорость движения
    private float MooveHorizontal_float = 0; // Направление движения вперёд-назад

    private float surfaceFriction_float = 0.2f; // Трение земли
    private float airResistance_float = 0.05f; // Сопротивление Воздуха

    private float timeJump_float = 0.5f; // Время между прыжками
    private float timerJump_float = 0; // Таймер времени между прыжками

    //private float leftSideScreen_float; // Количество пикселей до центра экрана

    private bool isGraundet_bool; // Персонаж на земле?
    private bool jumped_bool = false; // Персонаж прыгнул?


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //leftSideScreen_float = Screen.width / 2;
    }


    void Update()
    {
        timerJump_float += Time.deltaTime;

        if (Input.GetKey(MooveJump_KeyCode))
            if (isGraundet_bool)
                jumped_bool = true;

        if (Input.GetKeyUp(MooveJump_KeyCode))
            jumped_bool = false;
        

        if(Input.GetKey(MooveSquat_KeyCode))
        {
            Body.localScale = Vector3.Lerp(Body.localScale, new Vector3(Body.localScale.x, 0.65f, Body.localScale.z), 0.1f);
        }
        else
        {
            Body.localScale = Vector3.Lerp(Body.localScale, Vector3.one, 0.1f);
        }

        // Проверка нормали земли
        Debug.DrawRay(transform.position, groundNormal * 20, Color.yellow);
        Debug.DrawRay(transform.position, -groundNormal * 20, Color.yellow);
    }


    private void FixedUpdate()
    {
        // Управление передвижения
        MooveHorizontal_float = 0;

        if (Input.GetKey(MooveRight_KeyCode))
            MooveHorizontal_float += 1;

        if (Input.GetKey(MooveLeft_KeyCode))
            MooveHorizontal_float -= 1;

        if (!Input.GetKey(MooveLeft_KeyCode) && !Input.GetKey(MooveRight_KeyCode))
            MooveHorizontal_float = 0;


        if (isGraundet_bool)
        {
            if (jumped_bool)
            {
                if (timerJump_float > timeJump_float)
                {
                    Debug.Log("jump");
                    timerJump_float = 0;
                    jumped_bool = false;
                    rigidbody.AddForce(0, jump_float - rigidbody.velocity.y, 0, ForceMode.VelocityChange);
                }
            }else
            {
                // Управление на земле
                rigidbody.AddForce(groundNormal * MooveHorizontal_float * speedMoove_float, ForceMode.VelocityChange); // Движение
                rigidbody.velocity -= new Vector3(rigidbody.velocity.x * surfaceFriction_float, rigidbody.velocity.y * surfaceFriction_float, 0); // Замедление
            }
        }
        else
        {
            // Управление в воздухе
            rigidbody.AddForce(groundNormal * MooveHorizontal_float * (speedMoove_float / 4), ForceMode.VelocityChange); // Движение
            rigidbody.velocity -= new Vector3(rigidbody.velocity.x * airResistance_float, 0.5f, 0); // Замедление
        }


        // Вектора поворота
        Vector3 left = (Vector3.right + Vector3.forward).normalized;
        Vector3 right = (Vector3.left + Vector3.forward).normalized;

        // Проверка направлений поворота
        Debug.DrawRay(transform.position, left * 20, Color.blue);
        Debug.DrawRay(transform.position, right * 20, Color.red);

        // Поворот в сторону оружия
        if (Input.mousePosition.x < Screen.width / 2)
        {
            transform.LookAt(transform.position + Vector3.MoveTowards(transform.forward, left, 0.2f)); // Поворот тела
        }
        else
        {
            transform.LookAt(transform.position + Vector3.MoveTowards(transform.forward, right, 0.2f)); // Поворот тела
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (Vector3.Angle(Vector3.up, collision.contacts[0].normal) < 45)
        {
            groundNormal = Vector3.Cross(collision.contacts[0].normal, Vector3.back);

            isGraundet_bool = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        groundNormal = Vector3.left;
        isGraundet_bool = false;
    }
}
