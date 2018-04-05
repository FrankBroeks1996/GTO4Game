using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public Player Player;
    public int MovementCount = 1;
    public Tile tile;
    public int Health;
    public int Damage;
    public int Price;
    public bool CanMoveInTurn = true;
    public bool CanAttackInTurn = true;
}
