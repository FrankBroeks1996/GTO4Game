using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour {

    public Grid TileGrid;
    public MovementHandler MovementHandler;

    public void HandlePlayerHarvesters(Player player)
    {
        List<Unit> harvesters = TileGrid.GetAllPlayerHarvesters(player);
        foreach (Unit harvester in harvesters)
        {
            MoveHarvesterToNode(harvester);
        }
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
            Tile bestTile = TileGrid.GetBestTile(harvester.Destination, possibleTiles);

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
