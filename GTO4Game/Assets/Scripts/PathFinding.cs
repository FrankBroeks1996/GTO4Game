using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PathFinding : MonoBehaviour {

    [Header("The tile grid")]
    public Grid TileGrid;

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

    public Tile GetStart(Unit unit)
    {
        Tile start = unit.transform.parent.GetComponent<Tile>();
        return start;
    }

    public void MoveHarvester(Unit unit, List<Tile> path)
    {
        
        if(path.Count <= unit.MovementRange + 1)
        {
            unit.Move(path[1]);
            HandleHarvesterDestination(unit);
        }
        else
        {
            unit.Move(path[path.Count - (unit.MovementRange + 1)]);
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
}
