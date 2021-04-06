using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float LiveTime;
    [SerializeField] private float PowerShot;

    [SerializeField] private GameObject flash;
    [SerializeField] private Transform spawn;
    [SerializeField] private AudioSource Audio;

    private GameObject bullet;

    private float timerShot = 0.3f;
    private float timeShot;


    void Start()
    {
        bullet = Resources.Load("Prefabs/Bullet", typeof(GameObject)) as GameObject;
    }

    
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
        newBullet.GetComponent<Rigidbody>().velocity = spawn.forward * PowerShot;
        Destroy(newBullet, LiveTime);

        Audio.pitch = Random.Range(0.9f, 1.1f);
        Audio.Play();

        flash.SetActive(true);
        Invoke("HideFlash", 0.08f);
    }

    private void HideFlash()
    {
        flash.SetActive(false);
    }

}
