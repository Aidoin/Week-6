using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoove : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private float Distance;

    
    void Update()
    {
        transform.position = new Vector3(Player.position.x, Player.position.y, Player.position.z + Distance);
    }
}
