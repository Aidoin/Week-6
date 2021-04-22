using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup))]

public class ConsolMassage : MonoBehaviour
{

    public Text textMassage;

    private RectTransform rectTransfrom;
    private VerticalLayoutGroup qwe;
    private Color ColorMassage;

    private float timeLive = 0;
    private float allottedTimeToLive = 5;

    private void Awake()
    {
        rectTransfrom = GetComponent<RectTransform>();
        qwe = GetComponent<VerticalLayoutGroup>();
    }

    private void Start()
    {
        ColorMassage = textMassage.color;
    }


    void Update()
    {
        timeLive += Time.deltaTime;

        if (timeLive > allottedTimeToLive)
        {
            ColorMassage.a -= Time.deltaTime;

            textMassage.color = ColorMassage;

            if (textMassage.color.a < 0)
            {
                rectTransfrom.sizeDelta = new Vector2(rectTransfrom.sizeDelta.x, rectTransfrom.sizeDelta.y - Time.deltaTime * 20);
                if (rectTransfrom.sizeDelta.y < 0)
                    Destroy(gameObject, 0.2f);
            }
        }
    }
}