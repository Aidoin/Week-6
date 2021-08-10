using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int segments = 10;
    [SerializeField] private Transform pointA, pointB;
    [SerializeField] private float lenght;


    void Update()
    {
        Draw(pointA.position, pointB.position, lenght);
    }


    private void Draw(Vector3 a, Vector3 b, float lenght)
    {
        lineRenderer.enabled = true;

        float interpolant = Vector3.Distance(a, b) / lenght;
        float offset = Mathf.Lerp(lenght / 2, 0, interpolant);

        Vector3 a_down = a + Vector3.down * offset;
        Vector3 b_down = b + Vector3.down * offset;

        lineRenderer.positionCount = segments + 1;
        for (int i = 0; i < segments + 1; i++)
        {
            lineRenderer.SetPosition(i, Bezier.GetPoint(a, a_down, b_down, b, (float)i / segments));
        }
    }
}