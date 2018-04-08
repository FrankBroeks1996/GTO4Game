using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SelectionType
{
    Build,
    Structure,
    Unit,
    None
}

public class SelectionHandler : MonoBehaviour {
    public ChangeScreen ChangeScreenHandler;
    public PlayerManager PlayerManager;
    public MovementHandler MovementHandler;
    public Camera MainCamera;
    public Unit SelectedUnit;
    public Structure SelectedStructure;
    public GameObject BuildFactory;
    public Grid TileGrid;
    public SelectionType SelectionType = SelectionType.None;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (!IsPointerOverUIObject())
            {
                if (Physics.Raycast(ray, out raycastHit, 100f, LayerMask.GetMask("Grid")))
                {
                    Tile clickedTile = raycastHit.collider.GetComponent<Tile>();
                    if (clickedTile != null)
                    {
                        if (clickedTile.ArmyEntityOnTile != null && clickedTile.ArmyEntityOnTile.Player != null && clickedTile.ArmyEntityOnTile.Player == PlayerManager.PlayerInTurn)
                        {
                            if (clickedTile.ArmyEntityOnTile is Unit)
                            {
                                ResetSelection();
                                SelectionType = SelectionType.Unit;
                                SelectedUnit = (Unit)clickedTile.ArmyEntityOnTile;
                                ChangeScreenHandler.SetAllScreensInactive();


                                List<Tile> possibleTiles = new List<Tile>();
                                if (SelectedUnit.CanMoveInTurn)
                                {
                                    possibleTiles.AddRange(TileGrid.GetTilesWithinMovementRange(clickedTile, SelectedUnit.MovementCount));
                                }
                                if (SelectedUnit.CanAttackInTurn)
                                {
                                    possibleTiles.AddRange(TileGrid.GetTilesWithinAttackRange(clickedTile, SelectedUnit.MovementCount));
                                }
                                foreach (Tile tile in possibleTiles)
                                {
                                    if (tile.ArmyEntityOnTile != null && tile.ArmyEntityOnTile.Player != PlayerManager.PlayerInTurn)
                                    {
                                        tile.HighLight(true, Color.red);
                                    }
                                    else if (!tile.Occupied)
                                    {
                                        tile.HighLight(true);
                                    }
                                }
                            }
                            else if (clickedTile.ArmyEntityOnTile is Structure)
                            {
                                ResetSelection();
                                SelectionType = SelectionType.Structure;
                                SelectedStructure = (Structure)clickedTile.ArmyEntityOnTile;
                                ChangeScreenHandler.SwitchToCreateUnitScreen(SelectedStructure);
                            }
                        }
                        else if (clickedTile.ResourceNodeOnTile == null)
                        {
                            switch (SelectionType)
                            {
                                case SelectionType.Build:
                                    if(BuildFactory.GetComponent<StructureFactory>() != null)
                                    {
                                        List<Tile> buildableTiles = TileGrid.GetAllBuildableTiles(3);
                                        if (buildableTiles.Contains(clickedTile) && PlayerManager.PlayerInTurn.Resources.RemoveResources(BuildFactory.GetComponent<StructureFactory>().Structure.GetComponent<Structure>().Price))
                                        {
                                            BuildFactory.GetComponent<StructureFactory>().InstantiateStructure(clickedTile);
                                        }
                                        ResetSelection(SelectionType.Structure);
                                    }
                                    else if(BuildFactory.GetComponent<UnitFactory>() != null)
                                    {
                                        List<Tile> spawnableTiles = TileGrid.GetStructureSpawnableTiles(SelectedStructure);
                                        if(spawnableTiles.Contains(clickedTile) && PlayerManager.PlayerInTurn.Resources.RemoveResources(BuildFactory.GetComponent<UnitFactory>().unit.GetComponent<Unit>().Price))
                                        {
                                            BuildFactory.GetComponent<UnitFactory>().InstantiateUnit(clickedTile);
                                            clickedTile.HighLight(false);
                                        }
                                    }
                                    break;
                                case SelectionType.Unit:
                                    if (TileGrid.IsTileWithinRange(SelectedUnit.transform.parent.GetComponent<Tile>(), SelectedUnit.MovementCount, clickedTile))
                                    {
                                        if (clickedTile.ArmyEntityOnTile == null)
                                        {
                                            MovementHandler.Move(SelectedUnit, clickedTile);
                                        }
                                        else
                                        {
                                            MovementHandler.AttackArmyEntity(SelectedUnit, clickedTile.ArmyEntityOnTile);
                                        }
                                        ResetSelection();
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                ResetSelection();
            }
        }
	}
    
    public void SelectBuild(GameObject factory)
    {
        SelectionType = SelectionType.Build;
        BuildFactory = factory;
        SelectedUnit = null;

        if(BuildFactory.GetComponent<StructureFactory>() != null)
        {
            List<Tile> buildableTiles = TileGrid.GetAllBuildableTiles(3);
            foreach (Tile tile in buildableTiles)
            {
                tile.HighLight(true, Color.green);
            }
        }
        else if(BuildFactory.GetComponent<UnitFactory>() != null)
        {
            List<Tile> spawnableTiles = TileGrid.GetStructureSpawnableTiles(SelectedStructure);
            foreach (Tile tile in spawnableTiles)
            {
                tile.HighLight(true, Color.green);
            }
        }
        
    }

    public void ResetSelection(SelectionType selectionType = SelectionType.None)
    {
        TileGrid.TurnAllHighlightOf();
        SelectionType = selectionType;
        SelectedUnit = null;
        SelectedStructure = null;
        BuildFactory = null;
        if(selectionType == SelectionType.None)
        {
            ChangeScreenHandler.SwitchToBuildStructureScreen(PlayerManager.PlayerInTurn.PlayerUI);
        }
    }

    //When Touching UI
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
