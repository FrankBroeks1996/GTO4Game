using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureFactory : MonoBehaviour {

    public Player Player;
    public GameObject Structure;
    public Grid TileGrid;

    public GameObject InstantiateStructure(Tile tile)
    {
        GameObject newStructure = Instantiate(Structure, new Vector3(tile.transform.position.x, 3, tile.transform.position.z), Quaternion.identity);
        newStructure.SetActive(true);
        newStructure.transform.parent = tile.transform;
        tile.StructureOnTile = newStructure.GetComponent<Structure>();
        tile.Occupied = true;
        return newStructure;
    }
}
