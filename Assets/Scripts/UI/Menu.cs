using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Menu : MonoBehaviour
{

    [SerializeField] private KeyBinding keyBinding;

    [SerializeField] private GameObject gameInterface;
    [SerializeField] private GameObject menu;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Toggle toggleBackgroundMusic;
    [SerializeField] private Slider sliderVolumeMusic;

    private Hub hub;

    private bool isShowed = false;


    private void Awake()
    {
        hub = FindObjectOfType<Hub>();

        // Устанавливаем значение в меню
        menu.SetActive(true);
        float volume;
        audioMixer.GetFloat("Master", out volume);
        sliderVolumeMusic.value = volume;
        menu.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(keyBinding.Escape))
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


    public void SetVolumeMusicBacground(float volume)
    {

        audioMixer.SetFloat("Master", volume);
        sliderVolumeMusic.value = volume;
    }


    public void UpdateVolumeMusicBacground()
    {
        //if (sliderVolumeMusic.value == -20)
        //{
        //    audioMixer.SetFloat("Master", -80);
        //}
        //else
        //{
        if(sliderVolumeMusic.value > -15)
        {
            audioMixer.SetFloat("Master", sliderVolumeMusic.value * 40);
        }
        else
            audioMixer.SetFloat("Master", sliderVolumeMusic.value * 20);
        //}
    }


    public void SwitchOnOffBackgroundMusic()
    {
        if (toggleBackgroundMusic.isOn)
            audioMixer.SetFloat("Background", 0);
        else
            audioMixer.SetFloat("Background", -80);
    }


    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}