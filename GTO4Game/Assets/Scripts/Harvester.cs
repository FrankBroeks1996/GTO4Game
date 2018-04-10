using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester : Unit {

    [Header("Booleans that are used to controll what the harvester is going to do")]
    public bool HasResources = false;
    public bool Harvesting = false;

    [Header("The amout of resources a harvester can carry")]
    public int ResourceCarryAmount;
}
