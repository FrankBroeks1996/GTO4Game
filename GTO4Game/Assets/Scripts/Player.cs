using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Color color;
    public PlayerUI PlayerUI;
    public UnitFactory BaseFactory;
    public UnitFactory HarvesterFactory;
    public Resource Resources;
    public String Name;
}
