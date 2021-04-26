using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionInEditor : MonoBehaviour
{

    [SerializeField] private GameObject Object;
    [SerializeField] private bool setNullOfZ = false;

    private void Awake()
    {
        if (setNullOfZ)
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        Object.transform.parent = transform.parent;
        Destroy(gameObject);
    }
}