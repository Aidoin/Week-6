using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{

    [SerializeField] private MeshRenderer[] mashes;


    public void StartBlink(float time)
    {
        StartCoroutine(BlinkEffect(time));
    }


    private IEnumerator BlinkEffect(float time)
    {
        Color colorcurrentColor = Color.black;

        for (float t = 0; t < time; t += Time.deltaTime)
        {
            colorcurrentColor = new Color(Mathf.Sin(t * 10) * 0.5f + 0.5f, 0, 0, 0);

            SetEmissionColorMaterial(colorcurrentColor);
            yield return null;
        }

        // Возвращение цвета в нормальному состоянию
        for (float v = colorcurrentColor.r; v > 0; v -= Time.deltaTime * 5)
        {
            SetEmissionColorMaterial(new Color(v, 0, 0, 0));
            yield return null;
        }

        SetEmissionColorMaterial(Color.clear);
    }


    private void SetEmissionColorMaterial(Color newColor)
    {
        for (int i = 0; i < mashes.Length; i++)
        {
            for (int l = 0; l < mashes[i].materials.Length; l++)
            {
                mashes[i].materials[l].SetColor("_EmissionColor", newColor);
            }
        }
    }
}