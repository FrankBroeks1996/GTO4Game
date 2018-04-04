using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour {

    public Grid Grid;
    public MovementHandler MovementHandler;

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
                    harvester.Destination = Grid.GetTileWithBase(harvesterTile);
                }
                else
                {
                    harvester.Destination = Grid.GetClosestTileWithResourceNode(harvesterTile);
                }
            }

            Debug.Log(harvester.MovementCount);
            List<Tile> possibleTiles = Grid.GetTilesWithinMovementRange(harvesterTile, harvester.MovementCount);
            Tile bestTile = Grid.GetBestTile(harvester.Destination, possibleTiles);

            MovementHandler.Move(unit, bestTile);
            harvesterTile = unit.transform.parent.GetComponent<Tile>();
            if (Grid.GetDistanceBetweenTiles(harvesterTile, harvester.Destination) == 1)
            {
                if (!harvester.HasResources)
                {
                    harvester.Harvesting = true;
                }
                else
                {
                    harvester.HasResources = false;
                }
                harvester.Destination = null;
            }
        }
    }
}
