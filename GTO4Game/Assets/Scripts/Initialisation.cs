using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialisation : MonoBehaviour {

    public PlayerManager TurnManager;
    public GameObject ResourceNode;
    public Grid TileGrid;

    void Awake()
    {
        TileGrid.Initialize();
        InitializeBases();
        InitializeResourceNodes();
    }

    public void InitializeBases()
    {
        foreach (Player player in TurnManager.Players)
        {
            Tile tile = TileGrid.GetBaseSpawnPoint();
            player.BaseFactory.InstantiateStructure(tile);
        }
    }

    public void InitializeResourceNodes()
    {
        List<Tile> tiles = TileGrid.GetResourceNodeSpawnPoint();
        foreach (Tile tile in tiles)
        {
            GameObject resourceNode1 = Instantiate(ResourceNode, new Vector3(tile.transform.position.x, 3, tile.transform.position.z), Quaternion.identity);
            resourceNode1.transform.SetParent(tile.transform);
            tile.Occupied = true;
            tile.ResourceNodeOnTile = resourceNode1.GetComponent<ResourceNode>();
        }
    }
}
