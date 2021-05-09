using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : Gun
{

    private new void Awake()
    {
        base.Awake();
    }

    private new void Update()
    {
        if (isActive)
        {
            timeShot += Time.deltaTime;

            if (Input.GetKey(keyBinding.Shot) && timeShot > timerShot)
            {
                Shot();
            }
            else
            {
                if (Input.GetKeyDown(keyBinding.Shot) && numberOfBullets > 0)
                {
                    Shot();
                }
            }


            if (Input.GetKeyDown(keyBinding.Reloading) && numberOfBullets > 0)
            {
                Reloading();
            }

            // Курок отжат
            if (Input.GetKeyUp(keyBinding.Shot))
            {
                isPulledTrigger = false;
            }
        }
    }
}