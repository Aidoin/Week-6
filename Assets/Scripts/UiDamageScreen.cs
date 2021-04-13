using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class UiDamageScreen : MonoBehaviour
{

    [SerializeField] private float visibilityTime;

    private Image damageImade;


    private void Awake()
    {
        damageImade = GetComponent<Image>();
    }


    public void StartDamageScreen()
    { 
        StartCoroutine(DamageScreenEffect());
    }


    private IEnumerator DamageScreenEffect()
    {
        Color colorImage = damageImade.color;
        colorImage.a = 1;
        damageImade.color = colorImage;
        
        for (float i = 1; i > 0; i -= Time.deltaTime / visibilityTime)
        {
            colorImage.a = i;
            damageImade.color = colorImage;
            yield return null;
        }
    }
}
