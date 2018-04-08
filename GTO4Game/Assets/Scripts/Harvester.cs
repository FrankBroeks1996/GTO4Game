using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester : Unit {

    public bool HasResources = false;
    public bool Harvesting = false;
    public Tile Destination;
    public int ResourceCarryAmount;

    public Tile PrevTile;
}
