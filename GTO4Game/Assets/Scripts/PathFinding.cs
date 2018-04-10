using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PathFinding : MonoBehaviour {

    public Grid TileGrid;
    public MovementHandler MovementHandler;

    public void HandlePlayerHarvesters(Player player)
    {
        List<Unit> harvesters = TileGrid.GetAllPlayerHarvesters(player);
        foreach (Unit harvester in harvesters)
        {
            if (!IsHarvesterHarvesting(harvester))
            {
                Tile start = GetStart(harvester);
                List<Tile> path = GetPathToDestination(harvester, start);
                TileGrid.TurnAllHighlightOf();
                if (path != null)
                {
                    foreach (Tile tile in path)
                    {
                        tile.HighLight(true, Color.blue);
                    }
                    MoveHarvester(harvester, path);
                }
            }
        }
    }

    public List<Tile> ChooseBestPath(Tile start, Tile end)
    {
        TileGrid.ResetAllTilePathfinding();
        List<Tile> path = new List<Tile>();
        List<Tile> closedSet = new List<Tile>();
        List<Tile> openSet = new List<Tile>();

        openSet.Add(start);

        while (openSet.Count > 0)
        {
            int lowestIndex = 0;
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FScore < openSet[lowestIndex].FScore)
                {
                    lowestIndex = i;
                }
            }

            Tile current = openSet[lowestIndex];

            if (openSet[lowestIndex] == end)
            {
                Tile tempTile = current;

                path.Add(tempTile);
                while (tempTile.Previous)
                {
                    path.Add(tempTile.Previous);
                    tempTile = tempTile.Previous;
                }
                foreach (Tile tile in path)
                {
                    tile.HighLight(true, Color.blue);
                }
                return path;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Tile neighbour in current.Neighbours)
            {
                if (!closedSet.Contains(neighbour))
                {
                    if (neighbour == end || !neighbour.ArmyEntityOnTile)
                    {
                        int tempGScore = current.GScore + 1;
                        if (openSet.Contains(neighbour))
                        {
                            if (tempGScore < neighbour.GScore)
                            {
                                neighbour.GScore = tempGScore;
                            }
                        }
                        else
                        {
                            neighbour.GScore = tempGScore;
                            openSet.Add(neighbour);
                        }

                        neighbour.HScore = TileGrid.GetDistanceBetweenTiles(neighbour, end);
                        neighbour.FScore = neighbour.GScore + neighbour.HScore;
                        neighbour.Previous = current;
                    }
                }

            }


            TileGrid.TurnAllHighlightOf();
            foreach (Tile tile in openSet)
            {
                tile.HighLight(true, Color.green);
            }
            foreach (Tile tile in closedSet)
            {
                tile.HighLight(true, Color.red);
            }
        }
        return null;
    }

    public List<Tile> GetPathToClosestResourceNode(Tile start)
    {
        List<Tile> bestPath = null;
        List<Tile> tilesWithResourceNode = TileGrid.GetTilesWithResourceNode();
        foreach (Tile tile in tilesWithResourceNode)
        {
            List<Tile> path = ChooseBestPath(start, tile);
            if (path != null)
            {
                if (bestPath == null)
                {
                    bestPath = path;
                }
                else if (bestPath.Count > path.Count)
                {
                    bestPath = path;
                }
            }
        }
        return bestPath;
    }

    public List<Tile> GetPathToDestination(Unit unit, Tile start)
    {
        List<Tile> path = null;
        Harvester harvester = (Harvester)unit;
        if (harvester.HasResources)
        {
            path = ChooseBestPath(start, TileGrid.GetTileWithBase(start, harvester.Player));
        }
        else
        {
            path = GetPathToClosestResourceNode(start);
        }
        return path;
    }

    public Tile GetDestination(Unit unit)
    {
        Harvester harvester = (Harvester)unit;
        Tile harvesterTile = unit.transform.parent.GetComponent<Tile>();
        if (harvester.HasResources)
        {
            harvester.Destination = TileGrid.GetTileWithBase(harvesterTile, harvester.Player);
        }
        else
        {
            harvester.Destination = TileGrid.GetClosestTileWithResourceNode(harvesterTile);
        }
        return harvester.Destination;
    }

    public Tile GetStart(Unit unit)
    {
        Tile start = unit.transform.parent.GetComponent<Tile>();
        return start;
    }

    public void MoveHarvester(Unit unit, List<Tile> path)
    {
        
        if(path.Count <= unit.MovementCount + 1)
        {
            MovementHandler.Move(unit, path[1]);
            HandleHarvesterDestination(unit);
        }
        else
        {
            MovementHandler.Move(unit, path[path.Count - (unit.MovementCount + 1)]);
            if (unit.transform.parent.GetComponent<Tile>() == path[1])
            {
                HandleHarvesterDestination(unit);
            }
        }
    }

    public void HandleHarvesterDestination(Unit unit)
    {
        Harvester harvester = (Harvester)unit;
        if (!harvester.HasResources)
        {
            harvester.Harvesting = true;
            harvester.CanMoveInTurn = false;
        }
        else
        {
            harvester.Player.Resources.AddResources(harvester.ResourceCarryAmount);
            harvester.HasResources = false;
        }
    }

    public bool IsHarvesterHarvesting(Unit unit)
    {
        Harvester harvester = (Harvester)unit;
        if (harvester.Harvesting)
        {
            harvester.Harvesting = false;
            harvester.HasResources = true;
            return true;
        }
        return false;
    }

    public void MoveHarvesterToNode(Unit unit)
    {
        Harvester harvester = (Harvester)unit;
        
        if (harvester.Harvesting)
        {
            harvester.Harvesting = false;
            harvester.HasResources = true;
        }
        else
        {
            Tile harvesterTile = unit.transform.parent.GetComponent<Tile>();
            if (harvester.Destination == null)
            {
                if (harvester.HasResources)
                {
                    harvester.Destination = TileGrid.GetTileWithBase(harvesterTile, harvester.Player);
                }
                else
                {
                    harvester.Destination = TileGrid.GetClosestTileWithResourceNode(harvesterTile);
                }
            }

            List<Tile> possibleTiles = TileGrid.GetTilesWithinMovementRange(harvesterTile, harvester.MovementCount);
            Tile bestTile = TileGrid.GetBestTile(harvester.Destination, possibleTiles, harvester.PrevTile);
            harvester.PrevTile = harvesterTile;

            MovementHandler.Move(unit, bestTile);
            harvesterTile = unit.transform.parent.GetComponent<Tile>();
            if (TileGrid.GetDistanceBetweenTiles(harvesterTile, harvester.Destination) == 1)
            {
                if (!harvester.HasResources)
                {
                    harvester.Harvesting = true;
                }
                else
                {
                    harvester.Player.Resources.AddResources(harvester.ResourceCarryAmount);
                    harvester.HasResources = false;
                }
                harvester.Destination = null;
            }
        }
    }
}
