using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DiscardingShot : MonoBehaviour
{
    [SerializeField] private KeyBinding keyBinding;

    [SerializeField] private Image image;
    [SerializeField] private Image loading;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AudioSource audioCooldown;
    [SerializeField] private AudioSource audioUse;
    [SerializeField] private GameObject effectAbility;

    [SerializeField] private float timerCooldownAbility;

    private float timeCooldown;
    private float timePlayerInAir;
    private bool landingRollback = false;


    private void Start()
    {
       timeCooldown = timerCooldownAbility;
    }


    private void Update()
    {
        timePlayerInAir += Time.deltaTime; // Время в воздухе увеличивается (Обнуляется если игрок не стоит на земле)

        if(timePlayerInAir > 1f) // Если игрок в воздухе больше 1 секунды
        {
            landingRollback = true;
        }

        if (timeCooldown < timerCooldownAbility) // Если время с последнего использования абилки меньше чем время перезарядки
        {
            timeCooldown += Time.deltaTime;
            loading.fillAmount = 1 - (timeCooldown / timerCooldownAbility);
        }
        else
        {
            loading.fillAmount = 0;
        }            
        
        if (Input.GetKeyDown(keyBinding.Ability_2))
        {
            if (timeCooldown < timerCooldownAbility)
            {
                audioCooldown.Play();
                return;
            }

            effectAbility.SetActive(true);
            playerRigidbody.velocity = -effectAbility.transform.forward * 22;
            timeCooldown = 0;
            audioCooldown.Stop();
            audioUse.Play();
        }
    }


    // Когда игрок стоит на земле
    public void PlayerIsGraundet()
    {
        timePlayerInAir = 0;

        if(landingRollback)
            ReadyToUse();
    }

    // Обнуляет перезарядку
    public void ReadyToUse()
    {
        timeCooldown = timerCooldownAbility;
        landingRollback = false;
    }
}