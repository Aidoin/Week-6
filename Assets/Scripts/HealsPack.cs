using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealsPack : MonoBehaviour
{

    [SerializeField] private GameObject effect;
    [SerializeField] private float amountOfTreatment = 1;

    private Hub hub;
    private bool isUsed = false;


    private void Awake()
    {
        hub = FindObjectOfType<Hub>();
    }


    private void Update()
    {
        transform.Rotate(Vector3.up, 50 * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.gameObject == hub.Player)
        {
            if (!isUsed)
            {
                isUsed = true;
                other.attachedRigidbody.GetComponent<VitalSigns>().HealthRestore(amountOfTreatment);

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