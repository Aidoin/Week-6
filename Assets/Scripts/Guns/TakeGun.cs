using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeGun : MonoBehaviour
{

    [SerializeField] private GameObject effect;
    [SerializeField] private int weaponuNmber;
    [SerializeField] private int numberOfCartridges;

    private Hub hub;

    private bool isUsed = false;


    private void Awake()
    {
        hub = FindObjectOfType<Hub>();
    }


    private void Update()
    {
        transform.Rotate(Vector3.up, -50 * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.gameObject == hub.Player)
        {
            if (!isUsed)
            {
                isUsed = true;

                // Добавляем оружие если его ещё нет
                if (hub.ChangingWeapons.Weapons[weaponuNmber].IsPresent == false)
                {
                    GunsEquipment gunsEquipment = hub.ChangingWeapons.Weapons[weaponuNmber];
                    gunsEquipment.IsPresent = true;
                    hub.ChangingWeapons.Weapons[weaponuNmber] = gunsEquipment;

                    hub.ChangingWeapons.GetAWeapon(weaponuNmber);
                }

                // Пополнение боезапаса
                hub.ChangingWeapons.Weapons[weaponuNmber].Gun.GetComponent<Gun>().RefillAmmo(numberOfCartridges);
                
                // Обновление боезапаса текущего оружия
                hub.ChangingWeapons.Weapons[hub.ChangingWeapons.CurrentWeapons].Gun.GetComponent<Gun>().UpdatePanelAMMO();

                StartCoroutine(OnTake());
            }
        }
    }


    private IEnumerator OnTake()
    {
        Destroy(Instantiate(effect, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity), 1);


        while (transform.localScale.x > 0.1)
        {
            if (transform.localScale.x < 0.3f)
            {
                Destroy(gameObject);
            }

            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.2f);

            yield return new WaitForFixedUpdate();
        }
    }
}