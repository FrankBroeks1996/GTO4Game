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
    public Camera MainCamera;
    public Unit SelectedUnit;
    public GameObject BuildFactory;
    public Grid TileGrid;
    public SelectionType SelectionType = SelectionType.None;
    public SelectionInfoUI SelectionInfoUI;
    public Unit HighlightedEntity;
	
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
                        if (clickedTile.ResourceNodeOnTile == null)
                        {
                            if (SelectionType == SelectionType.Build)
                            {
                                Build(clickedTile);
                            }
                        }

                        if(!HandleSelectedUnitAction(clickedTile))
                        {
                            if (clickedTile.ArmyEntityOnTile != null)
                            {
                                if (clickedTile.ArmyEntityOnTile.Player != PlayerManager.PlayerInTurn && SelectedUnit == null && BuildFactory == null)
                                {
                                    Unit unit = clickedTile.ArmyEntityOnTile;
                                    SetSelectionInfo(unit);
                                }

                                if (clickedTile.ArmyEntityOnTile.Player == PlayerManager.PlayerInTurn)
                                {
                                    ResetSelection();
                                    if (clickedTile.ArmyEntityOnTile is Structure)
                                    {
                                        Structure selectedStructure = (Structure)clickedTile.ArmyEntityOnTile;
                                        SelectionType = SelectionType.Structure;
                                        ChangeScreenHandler.SwitchToCreateUnitScreen(selectedStructure);
                                    }
                                    else
                                    {
                                        SelectionType = SelectionType.Unit;
                                        ChangeScreenHandler.SetAllScreensInactive();
                                    }
                                    SelectedUnit = clickedTile.ArmyEntityOnTile;
                                    SetSelectionInfo(SelectedUnit);
                                    HighLightPossibleMoveTiles(clickedTile);
                                    HighLightPossibleAttackTiles(clickedTile);
                                }
                            }
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

    public void SetSelectionInfo(Unit unit)
    {
        if (unit.Health > 0)
        {
            ResetHighlight();
            HighlightedEntity = unit;
            HighlightedEntity.HighLight(true);
            SelectionInfoUI.SetText(unit.Health, unit.Damage);
        }
    }

    public void HighLightPossibleAttackTiles(Tile clickedTile)
    {
        List<Tile> possibleAttackTiles = new List<Tile>();
        if (SelectedUnit.CanAttackInTurn)
        {
            possibleAttackTiles.AddRange(TileGrid.GetTilesWithinAttackRange(clickedTile, SelectedUnit.AttackRange));
            foreach (Tile tile in possibleAttackTiles)
            {
                if (tile.ArmyEntityOnTile != null && tile.ArmyEntityOnTile.Player != PlayerManager.PlayerInTurn)
                {
                    tile.HighLight(true, Color.red);
                }
            }
        }
    }
    public void HighLightPossibleMoveTiles(Tile clickedTile)
    {
        List<Tile> possibleMoveTiles = new List<Tile>();
        if (SelectedUnit.CanMoveInTurn)
        {
            possibleMoveTiles.AddRange(TileGrid.GetTilesWithinMovementRange(clickedTile, SelectedUnit.MovementRange));
            foreach (Tile tile in possibleMoveTiles)
            {
                if (!tile.Occupied)
                {
                    tile.HighLight(true);
                }
            }
        }
    }

    public bool HandleSelectedUnitAction(Tile clickedTile)
    {
        TileGrid.TurnAllHighlightOf();
        if (SelectedUnit != null)
        {
            if (clickedTile.ArmyEntityOnTile == null && TileGrid.IsTileWithinRange(SelectedUnit.transform.parent.GetComponent<Tile>(), SelectedUnit.MovementRange, clickedTile))
            {
                SelectedUnit.Move(clickedTile);
                return true;
            }
            else if (clickedTile.ArmyEntityOnTile != null && clickedTile.ArmyEntityOnTile.Player != PlayerManager.PlayerInTurn && TileGrid.IsTileWithinRange(SelectedUnit.transform.parent.GetComponent<Tile>(), SelectedUnit.AttackRange, clickedTile))
            {
                SelectedUnit.AttackEnemy(clickedTile.ArmyEntityOnTile);
                return true;
            }
        }
        return false;
    }

    public void Build(Tile clickedTile)
    {
        UnitFactory unitFactory = BuildFactory.GetComponent<UnitFactory>();
        if (unitFactory != null)
        {
            Unit unit = unitFactory.unit.GetComponent<Unit>();
            if (unit is Structure)
            {
                List<Tile> buildableTiles = TileGrid.GetAllBuildableTiles(3);
                if (buildableTiles.Contains(clickedTile) && PlayerManager.PlayerInTurn.Resources.RemoveResources(unit.Price))
                {
                    unitFactory.InstantiateUnit(clickedTile);
                }
                ResetSelection(SelectionType.Structure);
            }
            else
            {
                List<Tile> spawnableTiles = TileGrid.GetStructureSpawnableTiles((Structure)SelectedUnit);
                if (spawnableTiles.Contains(clickedTile) && PlayerManager.PlayerInTurn.Resources.RemoveResources(unit.Price))
                {
                    unitFactory.InstantiateUnit(clickedTile);
                    clickedTile.HighLight(false);
                }
            }
        }
    }
    
    public void SelectBuild(GameObject factory)
    {
        SelectionInfoUI.SetText();
        SelectionType = SelectionType.Build;
        BuildFactory = factory;

        UnitFactory unitFactory = BuildFactory.GetComponent<UnitFactory>();
        Unit unit = unitFactory.unit.GetComponent<Unit>();
        if (unit is Structure)
        {
            List<Tile> buildableTiles = TileGrid.GetAllBuildableTiles(3);
            foreach (Tile tile in buildableTiles)
            {
                tile.HighLight(true, Color.green);
            }
        }
        else
        {
            List<Tile> spawnableTiles = TileGrid.GetStructureSpawnableTiles(SelectedUnit.GetComponent<Structure>());
            foreach (Tile tile in spawnableTiles)
            {
                tile.HighLight(true, Color.green);
            }
        }
        
    }

    public void ResetSelection(SelectionType selectionType = SelectionType.None)
    {
        SelectionInfoUI.HideScreen();
        TileGrid.TurnAllHighlightOf();
        SelectionType = selectionType;
        SelectedUnit = null;
        BuildFactory = null;
        if(selectionType == SelectionType.None)
        {
            ChangeScreenHandler.SwitchToBuildStructureScreen(PlayerManager.PlayerInTurn.PlayerUI);
        }
        ResetHighlight();
    }

    public void ResetHighlight()
    {
        if (HighlightedEntity != null)
        {
            HighlightedEntity.HighLight(false);
            HighlightedEntity = null;
        }
    }
    
    private bool IsPointerOverUIObject()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
