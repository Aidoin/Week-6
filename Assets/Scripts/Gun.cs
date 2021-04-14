using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject flash;
    [SerializeField] private Transform spawn;
    [SerializeField] private AudioSource audioShot;

    [SerializeField] private float liveTime;
    [SerializeField] private float speedBulet;

    private float timerShot = 0.3f;
    private float timeShot;


    void Update()
    {
        timeShot += Time.deltaTime;


        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shot();
        }

        if (Input.GetKey(KeyCode.Mouse0) && timeShot > timerShot)
        {
            Shot();
        }
    }


    private void Shot()
    {
        timeShot = 0;

        GameObject newBullet = Instantiate(bullet, spawn.position, spawn.rotation);

        newBullet.GetComponent<Projectile>().team = 0;
        newBullet.GetComponent<DealDamageTouch>().Team = 0;
        newBullet.GetComponent<Rigidbody>().velocity = spawn.forward * speedBulet;

        Destroy(newBullet, liveTime);

        audioShot.pitch = Random.Range(0.9f, 1.1f);
        audioShot.Play();

        flash.SetActive(true);
        Invoke("HideFlash", 0.08f);
    }


    private void HideFlash()
    {
        flash.SetActive(false);
    }
}