using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Player color")]
    public Color color;

    [Header("The UI that controls the player")]
    public PlayerUI PlayerUI;

    [Header("Factory's for initial initilisation")]
    public UnitFactory BaseFactory;
    public UnitFactory HarvesterFactory;

    [Header("Resource prefab")]
    public Resource Resources;

    [Header("Player name")]
    public String Name;
}
