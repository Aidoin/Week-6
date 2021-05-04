using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VitalSigns))]
[RequireComponent(typeof(Rigidbody))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform Body;
    [SerializeField] private Transform Eyes;
    [SerializeField] private Transform Aim;
    [SerializeField] private KeyBinding keyBinding;
    [SerializeField] private AudioSource AudioDeath;

    private Rigidbody rigidbody;
    private VitalSigns vitalSigns;

    private Vector3 groundNormal; // Вектор движения персонажа относительно его положения
    private Vector3 left = (Vector3.right + Vector3.forward).normalized; // Вектор направления игрока влево
    private Vector3 right = (Vector3.left + Vector3.forward).normalized; // Вектор направления игрока вправо

    private float jumpPowert = 15; // Сила прыжка
    private float speedMoove = 2; // Скорость движения
    private float MooveHorizontal = 0; // Направление движения вперёд-назад

    private float surfaceFriction = 0.2f; // Трение земли
    private float airResistance = 0.05f; // Сопротивление Воздуха

    private float timeJump = 0.5f; // Время между прыжками
    private float timerJump = 0; // Таймер времени между прыжками

    public bool IsGraundet => isGraundet;
    private bool isGraundet; // Персонаж на земле?
    private bool isJumped = false; // Персонаж прыгнул?


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        vitalSigns = GetComponent<VitalSigns>();
    }


    void Update()
    {
        timerJump += Time.deltaTime;

        if (Input.GetKey(keyBinding.Jump))
            if (isGraundet)
                isJumped = true;

        if (Input.GetKeyUp(keyBinding.Jump))
            isJumped = false;
        

        if(Input.GetKey(keyBinding.Squat))
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

        // Проверка направлений поворота
        Debug.DrawRay(transform.position, left * 20, Color.blue);
        Debug.DrawRay(transform.position, right * 20, Color.red);

        // Поворот в сторону оружия
        if (Input.mousePosition.x < Screen.width / 2)
        {
            Body.LookAt(Body.position + Vector3.MoveTowards(Body.forward, left, Time.deltaTime * 20f)); // Поворот тела
        }
        else
        {
            Body.LookAt(Body.position + Vector3.MoveTowards(Body.forward, right, Time.deltaTime * 20f)); // Поворот тела
        }
    }


    private void FixedUpdate()
    {
        // Управление передвижения
        MooveHorizontal = 0;

        if (Input.GetKey(keyBinding.MoveRight))
            MooveHorizontal += 1;

        if (Input.GetKey(keyBinding.MoveLeft))
            MooveHorizontal -= 1;

        if (!Input.GetKey(keyBinding.MoveLeft) && !Input.GetKey(keyBinding.MoveRight))
            MooveHorizontal = 0;


        if (isGraundet)
        {
            if (isJumped)
            {
                if (timerJump > timeJump)
                {
                    timerJump = 0;
                    isJumped = false;
                    rigidbody.AddForce(0, jumpPowert - rigidbody.velocity.y, 0, ForceMode.VelocityChange);
                }
            }else
            {
                // Управление на земле
                rigidbody.AddForce(groundNormal * MooveHorizontal * speedMoove, ForceMode.VelocityChange); // Движение
                rigidbody.velocity -= new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0) * surfaceFriction; // Замедление
            }
        }
        else
        {
            // Управление в воздухе
            rigidbody.AddForce(groundNormal * MooveHorizontal * (speedMoove / 4), ForceMode.VelocityChange); // Движение
            rigidbody.velocity -= new Vector3(rigidbody.velocity.x * airResistance, 0.5f, 0); // Замедление
        }

        
        groundNormal = Vector3.left;
        isGraundet = false;
    }


    private void OnCollisionStay(Collision collision)
    {
        if (Vector3.Angle(Vector3.up, collision.contacts[0].normal) < 45)
        {
            groundNormal = Vector3.Cross(collision.contacts[0].normal, Vector3.back);

            isGraundet = true;
        }
    }
}