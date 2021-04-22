using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Console : MonoBehaviour
{

    [SerializeField] private GameObject consolMassage;


    public void ShowMassage(string massage)
    {
        Instantiate(consolMassage, transform).GetComponent<ConsolMassage>().textMassage.text = massage;
    }
}