using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SittingStandingPosition
{
    Stand,
    Squat
}


[RequireComponent(typeof(VitalSigns))]
[RequireComponent(typeof(Rigidbody))]


public class PlayerController : MonoBehaviour
{
    [HideInInspector] public bool isSpinning = false; // Крутиться в воздухе?

    [SerializeField] private Transform Body;
    [SerializeField] private Transform Eyes;
    [SerializeField] private Transform Aim;
    [SerializeField] private KeyBinding keyBinding;
    [SerializeField] private AudioSource AudioDeath;

    private new Rigidbody rigidbody;

    private Vector3 groundNormal; // Вектор движения персонажа относительно его положения
    private SittingStandingPosition sittingStanding; // Сидит или стоит?

    private float jumpPowert = 15; // Сила прыжка
    private float speedMoove = 2; // Скорость движения
    private float MooveHorizontal = 0; // Направление движения вперёд-назад

    private float surfaceFriction = 0.2f; // Трение земли
    private float airResistance = 0.05f; // Сопротивление Воздуха

    private float timeJump = 0.5f; // Время между прыжками
    private float timerJump = 0; // Таймер времени между прыжками
    private float opportunityTimerDisableRotation; // Через какое время после прыжка возможно отменить вращение (чтобы при прыжке сразу не отключалось вращение в онколлижен)

    public bool IsGraundet => isGraundet;
    private bool isGraundet; // Персонаж на земле?
    private bool isJumped = false; // Персонаж прыгнул?


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        sittingStanding = SittingStandingPosition.Stand;
    }


    void Update()
    {
        timerJump += Time.deltaTime;

        if (Input.GetKeyUp(keyBinding.Jump))
        {
            isJumped = false;
        }

        if (Input.GetKey(keyBinding.Squat))
        {
            sittingStanding = SittingStandingPosition.Squat;
        }
        else
        {
            sittingStanding = SittingStandingPosition.Stand;
        }

        // Поворот в сторону оружия
        TurnToTheSideOfTheWeapon();
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

        if (Input.GetKey(keyBinding.Jump))
        {
            if (isGraundet)
            {
                isJumped = true;
            }

            // Крутиться в прыжке
            Spinning();
        }

        if (isGraundet)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 15);

            if (isJumped)
            {
                Jump();
            }
            else
            {
                // Управление на земле
                rigidbody.AddForce(groundNormal * MooveHorizontal * speedMoove, ForceMode.VelocityChange); // Движение
                rigidbody.velocity -= new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, 0) * surfaceFriction; // Замедление
            }
        }
        else
        {
            rigidbody.freezeRotation = false;

            // Управление в воздухе
            rigidbody.AddForce(groundNormal * MooveHorizontal * (speedMoove / 4), ForceMode.VelocityChange); // Движение
            rigidbody.velocity -= new Vector3(rigidbody.velocity.x * airResistance, 0.5f, 0); // Замедление

            sittingStanding = SittingStandingPosition.Squat;
        }

        // Обновление положения Сидя/Стоя
        UpdatingPositionSittingStanding();

        // Обнуд=ление переменных
        groundNormal = Vector3.right;
        isGraundet = false;

        // Таймер
        opportunityTimerDisableRotation += Time.fixedDeltaTime;
    }


    private void Jump()
    {
        if (timerJump > timeJump)
        {
            timerJump = 0;
            opportunityTimerDisableRotation = 0;
            isJumped = false;
            rigidbody.freezeRotation = false;
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpPowert, rigidbody.velocity.z);
            isSpinning = true;
        }
    }


    private void UpdatingPositionSittingStanding()
    {
        if (sittingStanding == SittingStandingPosition.Stand)
        {
            Body.localScale = Vector3.Lerp(Body.localScale, Vector3.one, 0.2f); // Подняться

        }
        else
        {
            Body.localScale = Vector3.Lerp(Body.localScale, new Vector3(Body.localScale.x, 0.5f, Body.localScale.z), 0.2f); // Присесть
        }
    }


    private void TurnToTheSideOfTheWeapon()
    {
        Quaternion targetRotation;

        if (Input.mousePosition.x < Screen.width / 2)
        {
            targetRotation = Quaternion.Euler(new Vector3(0f, 45, 0f));
        }
        else
        {
            targetRotation = Quaternion.Euler(new Vector3(0f, -45, 0f));
        }

        Body.localRotation = Quaternion.Lerp(Body.localRotation, targetRotation, Time.deltaTime * 20f);
    }


    private void Spinning()
    {
        if (isSpinning)
        {
            // Поворот в воздухе
            if (rigidbody.velocity.x > 0)
            {
                rigidbody.AddRelativeTorque(0f, 0f, -10f, ForceMode.VelocityChange);
            }
            else
            {
                rigidbody.AddRelativeTorque(0f, 0f, 10f, ForceMode.VelocityChange);
            }
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (Vector3.Angle(Vector3.up, collision.contacts[0].normal) < 45)
        {
            groundNormal = Vector3.Cross(collision.contacts[0].normal, Vector3.forward);

            isGraundet = true;

            if (opportunityTimerDisableRotation > 0.2)
            {
                isSpinning = false;
                rigidbody.freezeRotation = true;
            }
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Проверка нормали земли
        Debug.DrawRay(transform.position, groundNormal * 20, Color.yellow);
        Debug.DrawRay(transform.position, -groundNormal * 20, Color.yellow);
    }
#endif
}