using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : Gun
{
    protected override void Shot()
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
            StartCoroutine(ReloadingRife());
        }
    }


    IEnumerator ReloadingRife()
    {
        yield return new WaitForSeconds(timerShot);
        Reloading();
    }
}