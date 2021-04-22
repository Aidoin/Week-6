﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : MonoBehaviour
{
    public GameObject Player;
    public PlayerValues PlayerValues;

    public Console Console;
}

public enum Direction
{
    Right,
    Left
}