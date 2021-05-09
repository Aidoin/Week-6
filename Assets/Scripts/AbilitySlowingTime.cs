using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AbilitySlowingTime : MonoBehaviour
{


    [SerializeField] private KeyBinding keyBinding;
    [SerializeField] private Image chargeImg;
    [SerializeField] private Image loadingImg;
    [SerializeField] private AudioSource audioCooldown;
    [SerializeField] private AudioSource audioUse;
    [SerializeField] private CameraMoove cameraMoove;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider chargeBar;


    [SerializeField] private float timerCooldownAbility;

    private float charge;
    private float timeCooldown;

    private bool isUsed = false;
    private bool isActive = false;


    private void Start()
    {
        charge = chargeImg.fillAmount;
        timeCooldown = timerCooldownAbility;
        audioUse.Play();
        audioUse.Pause();
    }


    private void Update()
    {
        // Кулдаун
        if (timeCooldown < timerCooldownAbility)
        {
            timeCooldown += Time.deltaTime;

            loadingImg.fillAmount = 1 - (timeCooldown / timerCooldownAbility);
        }
        else
        {
            loadingImg.fillAmount = 0;
        }

        // Переключение активности
        if (Input.GetKeyDown(keyBinding.Ability_1))
        {
            isActive = !isActive;

            if(isActive == false)
            {
                DisableAbility();

            }
        }

        // Использование способгности
        if(isActive)
        {
            if (timeCooldown < timerCooldownAbility)
            {
                audioCooldown.Play();
                DisableAbility();
                return;
            }
            else
            {
                EnableAbility();
            }
        }

        Ability();
    }

    float pitchAudioMixer = 1;
    private void Ability()
    {
        if (isUsed)
        {
            if (charge <= 0)
            {
                audioCooldown.Play();
                timeCooldown = 0;

                DisableAbility();
            }
            else
            {
                charge -= Time.deltaTime / 3;
                chargeImg.fillAmount = charge;
                chargeBar.value = charge;

                Time.timeScale = Mathf.MoveTowards(Time.timeScale, 0.4f, 0.05f);
                pitchAudioMixer = Mathf.MoveTowards(pitchAudioMixer, 0.6f, 0.05f);
                audioMixer.SetFloat("MasterPitch", pitchAudioMixer);
            }
        }
        else
        {
            ChargeRecovery();
        }
    }
    

    private void EnableAbility()
    {
        isUsed = true;
        audioUse.UnPause();
        cameraMoove.DistanceMax();
        chargeBar.gameObject.SetActive(true);
    }


    private void DisableAbility()
    {
        audioUse.Pause();
        isUsed = false;
        isActive = false;
        Time.timeScale = 1;
        cameraMoove.DistanceMin();
        chargeBar.gameObject.SetActive(false);

        pitchAudioMixer = 1;
        audioMixer.SetFloat("MasterPitch", pitchAudioMixer);
    }

    private void ChargeRecovery()
    {
        if (charge < 1)
        {
            charge += Time.deltaTime / 8;
            chargeImg.fillAmount = charge;
            chargeBar.value = charge;
        }
        else
        {
            charge = 1;
        }
    }
}