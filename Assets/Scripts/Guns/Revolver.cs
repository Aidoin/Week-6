using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : Gun
{

    private new void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        timeShot += Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse0) && timeShot > timerShot)
        {
            Shot();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && numberOfBullets > 0)
            {
                Shot();
            }
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
}