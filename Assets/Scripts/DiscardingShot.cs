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


    private void Start()
    {
       timeCooldown = timerCooldownAbility;
    }


    private void Update()
    {
        if (timeCooldown < timerCooldownAbility)
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
}