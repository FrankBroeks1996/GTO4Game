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

    private Tile[,] grid;

    // Use this for initialization
    void Start()
    {
        grid = new Tile[Size, Size];
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = Instantiate<Tile>(Tile, new Vector3(x + 1.5f * x, 1, y + 1.5f * y), Quaternion.identity);
            }
        }

        InstantiateBases();
        InstantiateResourceNodes();
        InstantiateResourceHarvester();
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

    public void InstantiateBases()
    {
        GameObject basePlayer1 = Instantiate(Base, new Vector3(grid[3, 3].transform.position.x, 3, grid[3, 3].transform.position.z), Quaternion.identity);
        basePlayer1.transform.SetParent(grid[3, 3].transform);
        grid[3, 3].Occupied = true;
        grid[3, 3].StructureOnTile = basePlayer1.GetComponent<Structure>();

        GameObject basePlayer2 = Instantiate(Base, new Vector3(grid[Size - 4, Size - 4].transform.position.x, 3, grid[Size - 4, Size - 4].transform.position.z), Quaternion.identity);
        basePlayer2.transform.SetParent(grid[Size - 4, Size - 4].transform);
        grid[Size - 4, Size - 4].Occupied = true;
        grid[Size - 4, Size - 4].StructureOnTile = basePlayer2.GetComponent<Structure>();
    }

    public void InstantiateResourceNodes()
    {
        GameObject resourceNode1 = Instantiate(ResourceNode, new Vector3(grid[0, 0].transform.position.x, 3, grid[0, 0].transform.position.z), Quaternion.identity);
        resourceNode1.transform.SetParent(grid[0, 0].transform);
        grid[0, 0].Occupied = true;
        grid[0, 0].ResourceNodeOnTile = resourceNode1.GetComponent<ResourceNode>();

        GameObject resourceNode2 = Instantiate(ResourceNode, new Vector3(grid[Size - 1, Size - 1].transform.position.x, 3, grid[Size - 1, Size - 1].transform.position.z), Quaternion.identity);
        resourceNode2.transform.SetParent(grid[Size - 1, Size - 1].transform);
        grid[Size - 1, Size - 1].Occupied = true;
        grid[Size - 1, Size - 1].ResourceNodeOnTile = resourceNode2.GetComponent<ResourceNode>();
    }

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

    //public void MergeTiles(Tile tile)
    //{
    //    Vector2 tilePos = GetGridTilePosition(tile);

    //    int count = 2;
    //    //float shiftAmount = 0.15f / count;
    //    float plusx = 0.3f;
    //    float plusy = 0.3f;

    //    for (int x = 0; x < count; x++)
    //    {
    //        for (int y = 0; y < count; y++)
    //        {
    //            grid[(int)tilePos.x + x, (int)tilePos.y + y].transform.position = new Vector3(grid[(int)tilePos.x + x, (int)tilePos.y + y].transform.position.x + (plusx - 0.15f), 1, grid[(int)tilePos.x + x, (int)tilePos.y + y].transform.position.z + (plusy - 0.15f));
    //            //plusy -= 
    //        }
    //        plusy = 0.3f;
    //        plusx -= 0.3f / count + (0.075f * count);
    //    }
    //}

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
}
