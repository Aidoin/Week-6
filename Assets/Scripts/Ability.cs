using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Ability : MonoBehaviour
{
    [SerializeField] private KeyCode useAbility;

    [SerializeField] private Image image;
    [SerializeField] private Image loading;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AudioSource AbilityCooldown;
    [SerializeField] private AudioSource AbilityUse;

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


        if (Input.GetKeyDown(useAbility))
        {
            if (playerController.IsGraundet)
            {
                return;
            }
            else if (timeCooldown < timerCooldownAbility)
            {
                AbilityCooldown.Play();
                return;
            }


            playerRigidbody.AddForce(Vector3.up * 20, ForceMode.VelocityChange);
            timeCooldown = 0;
            AbilityCooldown.Stop();
            AbilityUse.Play();
        }
    }
}