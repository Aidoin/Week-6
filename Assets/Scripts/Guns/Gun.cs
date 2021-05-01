using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gun : MonoBehaviour
{

    [SerializeField] protected GameObject bullet;
    [SerializeField] protected GameObject flash;
    [SerializeField] protected Transform spawnTransform;
    [SerializeField] protected AudioSource shotAudio;
    [SerializeField] protected AudioSource reloadingAudio;
    [SerializeField] protected AudioSource noneAMMOAudio;


    [SerializeField] protected float liveTime = 10;
    [SerializeField] protected float timerShot = 0.3f;
    [SerializeField] protected float timerReloading = 0.5f;
    [SerializeField] protected float speedBulet = 20;
    [SerializeField] protected float damage = 1;

    [SerializeField] protected float cartridgeMagazine = 6; // Размер магазина
    [SerializeField] protected float chargedInTheMagazine = 6; // Сколько патрон сейчас в магазине
    [SerializeField] protected float numberOfBullets = 50; // Общее количество патрон
    [SerializeField] protected float numberOfBulletsPerShot = 1; // Сколько патрон тратится при выстреле 

    protected AMMO uiPanelAMMO;

    protected float timeShot;
    protected bool isReloading = false;
    protected bool isPulledTrigger = false; // Нажат курок


    protected void Awake()
    {
        uiPanelAMMO = FindObjectOfType<AMMO>();
        timeShot = timerShot; // Приводит таймер в боевую готовность
    }


    protected void OnEnable()
    {
        UpdatePanelAMMO();
        isReloading = false; // Выключаем бесконечную перезарядку (если сменить оружие во время перезарядки, то оно как-бы бесконечно перезаряжается)
    }


    protected void Update()
    {
        timeShot += Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0) && timeShot > timerShot)
        {
            Shot();
        }

        if (Input.GetKeyDown(KeyCode.R) && numberOfBullets > 0)
        {
            Reloading();
        }

        // Курок отжат
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isPulledTrigger = false;
        }
    }


    protected virtual void Shot()
    {
        // Если игра на паузе
        if (Time.timeScale == 0)
            return;

        if (isReloading)
            return;

        if (chargedInTheMagazine == 0)
        {
            Reloading();
            return;
        }

        // Выстрел
        timeShot = 0;

        chargedInTheMagazine -= numberOfBulletsPerShot;
        UpdatePanelAMMO();

        GameObject newBullet = Instantiate(bullet, spawnTransform.position, spawnTransform.rotation);
        DealDamageTouch dealDamage = newBullet.GetComponent<DealDamageTouch>();

        newBullet.GetComponent<Projectile>().team = 0;
        newBullet.GetComponent<Rigidbody>().velocity = spawnTransform.forward * speedBulet;
        dealDamage.Team = 0;
        dealDamage.Damage = damage;

        Destroy(newBullet, liveTime);

        shotAudio.pitch = Random.Range(0.9f, 1.1f);
        shotAudio.Play();

        flash.SetActive(true);
        Invoke("HideFlash", 0.08f);

        if (chargedInTheMagazine == 0)
        {
            Reloading();
        }
    }


    protected void Reloading()
    {
        // Если магазин полный
        if (chargedInTheMagazine == cartridgeMagazine)
            return;

        if (numberOfBullets == 0) // Если патрон больше нет
        {
            if (isPulledTrigger == false) // Звук курка воспроизводится один раз при нажатии
            {
                isPulledTrigger = true;
                noneAMMOAudio.Play();
            }
            return;
        }

        if (isReloading == false)
        {
            StartCoroutine(StartReloading());
            isReloading = true;
        }
    }


    protected virtual void HideFlash()
    {
        flash.SetActive(false);
    }


    private IEnumerator StartReloading()
    {
        reloadingAudio.Play();

        yield return new WaitForSeconds(timerReloading);


        // Логика перезарядки
        float a = numberOfBullets - (cartridgeMagazine - chargedInTheMagazine); 
        if (a < 0)
        {
            chargedInTheMagazine = cartridgeMagazine + a;
            numberOfBullets = 0;
        }
        else
        {
            chargedInTheMagazine = cartridgeMagazine;
            numberOfBullets = a;
        }
        UpdatePanelAMMO();
        isReloading = false;
        timeShot = timerShot; // Чтобы после перезарядки сразу можно было выстрелить
    }


    public void UpdatePanelAMMO()
    {
        uiPanelAMMO.UpdatePanelAMMO(chargedInTheMagazine, numberOfBullets);
    }

    public void RefillAmmo(int value)
    {
        numberOfBullets += value;
    }
}