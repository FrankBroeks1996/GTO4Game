using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{

    public Player player;
    public GameObject unit;
    public Grid grid;

    public GameObject InstantiateUnit(Tile tile)
    {
        GameObject newUnit = Instantiate(unit, new Vector3(tile.x, 2, tile.y), Quaternion.identity);
        newUnit.transform.parent = tile.transform;
        newUnit.SetActive(true);
        tile.Occupied = true;
        tile.ArmyEntityOnTile = newUnit.GetComponent<Unit>();
        return newUnit;
    }
}
