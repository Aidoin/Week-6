using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AMMO : MonoBehaviour
{

    [SerializeField] private Text numberAMMO;
    [SerializeField] private Text totalAMMO; 

    private bool infinity = false;
    private int defaultTextSize;


    private void Awake()
    {
        defaultTextSize = numberAMMO.fontSize;
    }


    public void UpdatePanelAMMO(float chargedInTheMagazine, float numberOfAMMO)
    {
        if (numberOfAMMO == Mathf.Infinity)
        {
            infinity = true;
            totalAMMO.fontSize = 100;

            totalAMMO.text = "∞"; 
            numberAMMO.text = chargedInTheMagazine.ToString();
        }
        else
        {
            if (infinity)
            {
                totalAMMO.fontSize = defaultTextSize;
                infinity = false;
            }

            numberAMMO.text = chargedInTheMagazine.ToString();
            totalAMMO.text = numberOfAMMO.ToString(); 
        }
    }
}