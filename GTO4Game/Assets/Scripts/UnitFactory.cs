using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    [Header("The player the factory belongs to")]
    public Player player;

    [Header("The unit prefab the factory creates")]
    public GameObject unit;

    [Header("The tile grid")]
    public Grid grid;

    public GameObject InstantiateUnit(Tile tile)
    {
        GameObject newUnit = Instantiate(unit, new Vector3(tile.transform.position.x, 3, tile.transform.position.z), Quaternion.identity);
        newUnit.transform.parent = tile.transform;
        newUnit.SetActive(true);
        tile.Occupied = true;
        tile.ArmyEntityOnTile = newUnit.GetComponent<Unit>();
        return newUnit;
    }
}
