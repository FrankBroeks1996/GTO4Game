using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public int Size;
    public Tile Tile;
    public GameObject ResourceNode;
    public GameObject Base;
    public GameObject Harvester;
    public List<Player> Players;
    public PlayerManager TurnManager;

    private Tile[,] grid;

    public void Initialize()
    {
        grid = new Tile[Size, Size];
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = Instantiate<Tile>(Tile, new Vector3(x + 1.5f * x, 1, y + 1.5f * y), Quaternion.identity);
            }
        }
    }
    
    public Tile GetFirstEmptyTile()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if(!grid[x, y].Occupied)
                {
                    return grid[x, y];
                }
            }
        }

        return null;
    }

    public Tile GetBaseSpawnPoint()
    {
        Tile tile = null;
        if (!grid[3, 3].Occupied)
        {
            tile = grid[3, 3];
        }
        else if(!grid[Size - 4, Size - 4].Occupied)
        {
            tile = grid[Size - 4, Size - 4];
        }
        return tile;
    }

    public List<Tile> GetResourceNodeSpawnPoint()
    {
        List<Tile> tiles = new List<Tile>();
        tiles.Add(grid[0, 0]);
        tiles.Add(grid[Size - 1, Size - 1]);
        return tiles;
    }

    //public void InstantiateBases()
    //{
    //    GameObject basePlayer1 = Instantiate(Base, new Vector3(grid[3, 3].transform.position.x, 3, grid[3, 3].transform.position.z), Quaternion.identity);
    //    basePlayer1.transform.SetParent(grid[3, 3].transform);
    //    grid[3, 3].Occupied = true;
    //    grid[3, 3].StructureOnTile = basePlayer1.GetComponent<Structure>();

    //    GameObject basePlayer2 = Instantiate(Base, new Vector3(grid[Size - 4, Size - 4].transform.position.x, 3, grid[Size - 4, Size - 4].transform.position.z), Quaternion.identity);
    //    basePlayer2.transform.SetParent(grid[Size - 4, Size - 4].transform);
    //    grid[Size - 4, Size - 4].Occupied = true;
    //    grid[Size - 4, Size - 4].StructureOnTile = basePlayer2.GetComponent<Structure>();
    //}

    //public void InstantiateResourceNodes()
    //{
    //    GameObject resourceNode1 = Instantiate(ResourceNode, new Vector3(grid[0, 0].transform.position.x, 3, grid[0, 0].transform.position.z), Quaternion.identity);
    //    resourceNode1.transform.SetParent(grid[0, 0].transform);
    //    grid[0, 0].Occupied = true;
    //    grid[0, 0].ResourceNodeOnTile = resourceNode1.GetComponent<ResourceNode>();

    //    GameObject resourceNode2 = Instantiate(ResourceNode, new Vector3(grid[Size - 1, Size - 1].transform.position.x, 3, grid[Size - 1, Size - 1].transform.position.z), Quaternion.identity);
    //    resourceNode2.transform.SetParent(grid[Size - 1, Size - 1].transform);
    //    grid[Size - 1, Size - 1].Occupied = true;
    //    grid[Size - 1, Size - 1].ResourceNodeOnTile = resourceNode2.GetComponent<ResourceNode>();
    //}

    public void InstantiateResourceHarvester()
    {
        GameObject harvester = Instantiate(Harvester, new Vector3(grid[3, 8].transform.position.x, 3, grid[3, 8].transform.position.z), Quaternion.identity);
        harvester.transform.SetParent(grid[3, 8].transform);
        harvester.GetComponent<Unit>().Player = Players[0];
        grid[3, 8].Occupied = true;
        grid[3, 8].UnitOnTile = harvester.GetComponent<Unit>();
    }

    public int GetDistanceBetweenTiles(Tile startTile, Tile endTile)
    {
        Vector2 firstTile = new Vector2(-1, -1);
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == startTile || grid[x, y] == endTile)
                {
                    if (firstTile == new Vector2(-1, -1))
                    {
                        firstTile = new Vector2(x, y);
                    }
                    else
                    {
                        return (int)Mathf.Abs(firstTile.x - x) + (int)Mathf.Abs(firstTile.y - y);
                    }
                }
            }
        }
        return 0;
    }

    public Vector2 GetGridTilePosition(Tile tile)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == tile)
                {
                    return new Vector2(x, y);
                }
            }
        }
        return Vector2.zero;
    }

    public Tile GetClosestTileWithResourceNode(Tile fromTile)
    {
        Tile closestTile = null;
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if(grid[x, y].ResourceNodeOnTile != null)
                {
                    if(closestTile == null || GetDistanceBetweenTiles(fromTile, grid[x, y]) < GetDistanceBetweenTiles(closestTile, fromTile))
                    {
                        closestTile = grid[x, y];
                    }
                }
            }
        }
        return closestTile;
    }

    public Tile GetTileWithBase(Tile fromTile)
    {
        Tile closestTile = null;
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Structure structioreOnTile = grid[x, y].StructureOnTile;
                if (structioreOnTile != null && structioreOnTile.StructureType == StructureType.Base)
                {
                    if (closestTile == null || GetDistanceBetweenTiles(fromTile, grid[x, y]) < GetDistanceBetweenTiles(closestTile, fromTile))
                    {
                        closestTile = grid[x, y];
                    }
                }
            }
        }
        return closestTile;
    }

    public List<Tile> GetTilesWithinMovementRange(Tile startTile, int movementCount)
    {
        List<Tile> tilesWithinRange = new List<Tile>();
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (!grid[x, y].Occupied)
                {
                    float distance = GetDistanceBetweenTiles(startTile, grid[x, y]);
                    if(distance <= movementCount && distance > 0)
                    {
                        tilesWithinRange.Add(grid[x, y]);
                    }
                }
            }
        }
        return tilesWithinRange;
    }

    public List<Tile> GetTilesWithinAttackRange(Tile startTile, int attackRange)
    {
        List<Tile> targetsWithinRange = new List<Tile>();
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y].Occupied)
                {
                    float distance = GetDistanceBetweenTiles(startTile, grid[x, y]);
                    if (distance <= attackRange && distance > 0)
                    {
                        targetsWithinRange.Add(grid[x, y]);
                    }
                }
            }
        }
        return targetsWithinRange;
    }

    public Tile GetBestTile(Tile destination, List<Tile> possibleTiles)
    {
        Tile bestTile = null;
        foreach (Tile tile in possibleTiles)
        {
            if(bestTile == null || GetDistanceBetweenTiles(destination, tile) < GetDistanceBetweenTiles(destination, bestTile))
            {
                bestTile = tile;
            }
        }
        return bestTile;
    }

    public void TurnAllHighlightOf()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y].HighLight(false);
            }
        }
    }

    public bool IsTileWithinRange(Tile currentTile, int movementCount,Tile clickedTile)
    {
        List<Tile> possibleTiles = GetTilesWithinMovementRange(currentTile, movementCount);
        possibleTiles.AddRange(GetTilesWithinAttackRange(currentTile, movementCount));
        foreach (Tile tile in possibleTiles)
        {
            if(tile == clickedTile)
            {
                return true;
            }
        }
        return false;
    }

    public List<Tile> GetAllTilesWithPlayerStructure()
    {
        List<Tile> tilesWithPlayerStructure = new List<Tile>();
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y].StructureOnTile != null && grid[x, y].StructureOnTile.player == TurnManager.PlayerInTurn)
                {
                    tilesWithPlayerStructure.Add(grid[x, y]);
                }
            }
        }
        return tilesWithPlayerStructure;
    }

    public List<Tile> GetAllBuildableTiles(int buildRange)
    {
        List<Tile> tilesWithPlayerStructure = GetAllTilesWithPlayerStructure();
        List<Tile> buildableTiles = new List<Tile>();

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (!grid[x, y].Occupied)
                {
                    foreach (Tile tile in tilesWithPlayerStructure)
                    {
                        if(GetDistanceBetweenTiles(grid[x,y], tile) <= buildRange)
                        {
                            buildableTiles.Add(grid[x, y]);
                        }
                    }
                }
            }
        }
        return buildableTiles;
    }

    public List<Tile> GetAllTilesWithPlayerUnit(Player player)
    {
        List<Tile> tilesWithPlayerUnit = new List<Tile>();

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y].UnitOnTile != null && grid[x,y].UnitOnTile.Player == player)
                {
                    tilesWithPlayerUnit.Add(grid[x,y]);
                }
            }
        }
        return tilesWithPlayerUnit;
    }
}
