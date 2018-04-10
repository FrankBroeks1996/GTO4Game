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
    [Header("Every extra node adds 2 new nodes mirrored to each other")]
    public int ExtraResourceNodes;

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
        FillTileNeighbours();
    }

    public void FillTileNeighbours()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                List<Tile> neighbours = new List<Tile>();
                if(x - 1 >= 0)
                {
                    neighbours.Add(grid[x - 1, y]);
                }
                if(x + 1 < grid.GetLength(0))
                {
                    neighbours.Add(grid[x + 1, y]);
                }
                if (y - 1 >= 0)
                {
                    neighbours.Add(grid[x, y - 1]);
                }
                if (y + 1 < grid.GetLength(1))
                {
                    neighbours.Add(grid[x, y + 1]);
                }
                grid[x, y].Neighbours = neighbours;
            }
        }
    }

    public void ResetAllTilePathfinding()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y].ResetTilePathFinding();
            }
        }
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

    public List<Tile> GetResourceNodeSpawnPoints()
    {
        List<Tile> tiles = new List<Tile>();
        if (!tiles.Contains(grid[0, 0]))
        {
            tiles.Add(grid[0, 0]);
            tiles.Add(grid[Size - 1, Size - 1]);
        }

        int newResourceNodeCount = 0;
        while(newResourceNodeCount != ExtraResourceNodes)
        {
            int x = Random.Range(0, grid.GetLength(0) / 2);
            int y = Random.Range(0, grid.GetLength(1));
            if (!grid[x, y].Occupied)
            {
                tiles.Add(grid[x, y]);
                tiles.Add(grid[grid.GetLength(0) - (x + 1), grid.GetLength(1) - (y + 1)]);
                newResourceNodeCount++;
            }
        }
        
        return tiles;
    }

    public Tile GetHarvesterSpawnPoint()
    {
        Tile tile = null;
        if (!grid[3, 2].Occupied)
        {
            tile = grid[3, 2];
        }
        else if (!grid[Size - 4, Size - 3].Occupied)
        {
            tile = grid[Size - 4, Size - 3];
        }
        return tile;
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

    public List<Tile> GetTilesWithResourceNode()
    {
        List<Tile> tilesWithResourceNode = new List<Tile>();
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y].ResourceNodeOnTile != null)
                {
                    tilesWithResourceNode.Add(grid[x, y]);
                }
            }
        }
        return tilesWithResourceNode;
    }

    public Tile GetTileWithBase(Tile fromTile, Player player)
    {
        Tile closestTile = null;
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if(grid[x,y].ArmyEntityOnTile is Structure)
                {
                    Structure structureOnTile = (Structure)grid[x, y].ArmyEntityOnTile;
                    if (structureOnTile != null && structureOnTile.StructureType == StructureType.Base && structureOnTile.Player == player)
                    {
                        if (closestTile == null || GetDistanceBetweenTiles(fromTile, grid[x, y]) < GetDistanceBetweenTiles(closestTile, fromTile))
                        {
                            closestTile = grid[x, y];
                        }
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
                if (grid[x, y].ArmyEntityOnTile != null && grid[x, y].ArmyEntityOnTile.Player == TurnManager.PlayerInTurn && grid[x,y].ArmyEntityOnTile is Structure)
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

    public List<Tile> GetStructureSpawnableTiles(Structure structure)
    {
        List<Tile> tilesWithPlayerStructure = GetAllTilesWithPlayerStructure();
        List<Tile> spawnableTiles = new List<Tile>();

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (!grid[x, y].Occupied)
                {
                    foreach (Tile tile in tilesWithPlayerStructure)
                    {
                        Structure s = (Structure)tile.ArmyEntityOnTile;
                        if (s == structure)
                        {
                            if (GetDistanceBetweenTiles(grid[x, y], tile) <= structure.SpawnRange)
                            {
                                spawnableTiles.Add(grid[x, y]);
                            }
                        }
                    }
                }
            }
        }
        return spawnableTiles;
    }

    public List<Tile> GetAllTilesWithPlayerUnit(Player player)
    {
        List<Tile> tilesWithPlayerUnit = new List<Tile>();

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y].ArmyEntityOnTile != null && grid[x,y].ArmyEntityOnTile.Player == player)
                {
                    tilesWithPlayerUnit.Add(grid[x,y]);
                }
            }
        }
        return tilesWithPlayerUnit;
    }

    public List<Unit> GetAllPlayerHarvesters(Player player)
    {
        List<Unit> harvesters = new List<Unit>();
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if(grid[x,y].ArmyEntityOnTile != null && grid[x, y].ArmyEntityOnTile is Harvester && grid[x,y].ArmyEntityOnTile.Player == player)
                {
                    harvesters.Add(grid[x, y].ArmyEntityOnTile);
                }
            }
        }
        return harvesters;
    }
}
