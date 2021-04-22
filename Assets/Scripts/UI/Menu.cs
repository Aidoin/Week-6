using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{

    [SerializeField] private KeyCode escape;

    [SerializeField] private GameObject gameInterface;
    [SerializeField] private GameObject menu;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private Toggle toggleBackgroundMusic;
    [SerializeField] private Slider sliderVolumeMusic;

    private Hub hub;

    private bool isShowed = false;


    private void Awake()
    {
        hub = FindObjectOfType<Hub>();

        // Устанавливаем значение в меню
        menu.SetActive(true);
        sliderVolumeMusic.value = backgroundMusic.volume;
        menu.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(escape))
        {
            SwitchShowHideMenu();
        }
    }


    public void SwitchShowHideMenu()
    {
        isShowed = !isShowed;

        if (isShowed)
        {
            gameInterface.SetActive(false);
            menu.SetActive(true);
            Time.timeScale = 0;
            hub.PlayerValues.ChangingWeapons.enabled = false;
            hub.PlayerValues.PlayerController.enabled = false;
            hub.PlayerValues.Pointer.enabled = false;
        }
        else
        {
            gameInterface.SetActive(true);
            menu.SetActive(false);
            Time.timeScale = 1;
            hub.PlayerValues.ChangingWeapons.enabled = true;
            hub.PlayerValues.PlayerController.enabled = true;
            hub.PlayerValues.Pointer.enabled = true;
        }
    }


    public void SetVolumeMusic()
    {
        backgroundMusic.volume = sliderVolumeMusic.value;
    }


    public void SwitchOnOffBackgroundMusic()
    {
        if (toggleBackgroundMusic.isOn)
            backgroundMusic.Play();
        else
            backgroundMusic.Stop();
    }
}