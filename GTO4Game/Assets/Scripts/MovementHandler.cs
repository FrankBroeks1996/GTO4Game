using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour {

	public void Move(Unit unit, Tile tile)
    {
        Tile fromTile = unit.transform.parent.GetComponent<Tile>();
        fromTile.UnitOnTile = null;
        fromTile.Occupied = false;

        tile.UnitOnTile = unit;
        tile.Occupied = true;
        unit.transform.position = new Vector3(tile.transform.position.x, unit.transform.position.y, tile.transform.position.z);
        unit.transform.SetParent(tile.transform);
        unit.CanMoveInTurn = false;
    }

    public void UnitAttackUnit(int damage, GameObject target)
    {
        Unit unit = target.GetComponent<Unit>();
        Structure structure = target.GetComponent<Structure>();
        if(unit != null)
        {
            unit.Health -= damage;
            if (unit.Health <= 0)
            {
                Tile targetUnitTile = target.transform.parent.GetComponent<Tile>();
                targetUnitTile.Occupied = false;
                Destroy(target);
            }
        }else if(structure != null)
        {
            structure.Health -= damage;
            if (structure.Health <= 0)
            {
                Tile targetStructureTile = target.transform.parent.GetComponent<Tile>();
                targetStructureTile.Occupied = false;
                Destroy(target);
            }
        }
    }
}
