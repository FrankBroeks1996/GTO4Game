using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Unit : ArmyEntity {
    public int MovementCount = 1;
    public Tile tile;
    public bool CanMoveInTurn = true;
}
