using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour {

	public void Move(Unit unit, Tile tile)
    {
        if (unit.CanMoveInTurn)
        {
            Tile fromTile = unit.transform.parent.GetComponent<Tile>();
            fromTile.ArmyEntityOnTile = null;
            fromTile.Occupied = false;

            tile.ArmyEntityOnTile = unit;
            tile.Occupied = true;
            unit.transform.position = new Vector3(tile.transform.position.x, unit.transform.position.y, tile.transform.position.z);
            unit.transform.SetParent(tile.transform);
            unit.CanMoveInTurn = false;
        }
    }

    public void AttackArmyEntity(ArmyEntity performer, ArmyEntity target)
    {
        if (performer.CanAttackInTurn)
        {
            target.RemoveHealth(performer.Damage);
            performer.CanAttackInTurn = false;
        }
    }
}
