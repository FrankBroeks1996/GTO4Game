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
        TurnManager.InitializeBaseListeners(InitializeBases());
        InitializeHarvesters();
        InitializeResourceNodes();
    }

    public List<Structure> InitializeBases()
    {
        List<Structure> bases = new List<Structure>();
        foreach (Player player in TurnManager.Players)
        {
            Tile tile = TileGrid.GetBaseSpawnPoint();
            bases.Add(player.BaseFactory.InstantiateStructure(tile).GetComponent<Structure>());
        }
        return bases;
    }

    public void InitializeHarvesters()
    {
        foreach (Player player in TurnManager.Players)
        {
            Tile tile = TileGrid.GetHarvesterSpawnPoint();
            player.HarvesterFactory.InstantiateUnit(tile);
        }
    }

    public void InitializeResourceNodes()
    {
        List<Tile> tiles = TileGrid.GetResourceNodeSpawnPoints();
        foreach (Tile tile in tiles)
        {
            GameObject resourceNode1 = Instantiate(ResourceNode, new Vector3(tile.transform.position.x, 3, tile.transform.position.z), Quaternion.identity);
            resourceNode1.transform.SetParent(tile.transform);
            tile.Occupied = true;
            tile.ResourceNodeOnTile = resourceNode1.GetComponent<ResourceNode>();
        }
    }
}
