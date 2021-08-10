using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTexture : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float tX, tY, oX, oY, speedOffset;

    private float Offset;


    private void Start()
    {
        Offset = oY;
    }
    

    void Update()
    {
        Offset += Time.deltaTime * speedOffset;

        material.SetVector("_MainTex_ST", new Vector4(tX, tY, oX, Offset));
    }
}