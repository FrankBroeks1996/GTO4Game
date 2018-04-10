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

                        if (SelectedUnit != null)
                        {
                            if (clickedTile.ArmyEntityOnTile == null && TileGrid.IsTileWithinRange(SelectedUnit.transform.parent.GetComponent<Tile>(), SelectedUnit.MovementRange, clickedTile))
                            {
                                MovementHandler.Move(SelectedUnit, clickedTile);
                            }
                            else if (TileGrid.IsTileWithinRange(SelectedUnit.transform.parent.GetComponent<Tile>(), SelectedUnit.AttackRange, clickedTile))
                            {
                                if (clickedTile.ArmyEntityOnTile.Player != PlayerManager.PlayerInTurn)
                                {
                                    MovementHandler.AttackArmyEntity(SelectedUnit, clickedTile.ArmyEntityOnTile);
                                }
                            }
                            ResetSelection();
                        }else if (SelectedStructure != null && TileGrid.IsTileWithinRange(SelectedStructure.transform.parent.GetComponent<Tile>(), SelectedStructure.AttackRange, clickedTile))
                        {
                            if (clickedTile.ArmyEntityOnTile != null && clickedTile.ArmyEntityOnTile.Player != PlayerManager.PlayerInTurn)
                            {
                                MovementHandler.AttackArmyEntity(SelectedStructure, clickedTile.ArmyEntityOnTile);
                            }
                        }

                        if (clickedTile.ArmyEntityOnTile != null && clickedTile.ArmyEntityOnTile.Player != null)
                        {
                            if (clickedTile.ArmyEntityOnTile is Structure)
                            {
                                if (clickedTile.ArmyEntityOnTile.Player != PlayerManager.PlayerInTurn && SelectedUnit == null && SelectedStructure == null && BuildFactory == null)
                                {
                                    Structure structure = (Structure)clickedTile.ArmyEntityOnTile;
                                    if (structure.Health > 0)
                                    {
                                        ResetHighlight();
                                        HighlightedEntity = structure;
                                        HighlightedEntity.HighLight(true);
                                        SelectionInfoUI.SetText(structure.Health, structure.Damage);
                                    }
                                }
                                else if (clickedTile.ArmyEntityOnTile.Player == PlayerManager.PlayerInTurn)
                                {
                                    ResetSelection();
                                    SelectionType = SelectionType.Structure;
                                    SelectedStructure = (Structure)clickedTile.ArmyEntityOnTile;
                                    ChangeScreenHandler.SwitchToCreateUnitScreen(SelectedStructure);
                                    SelectionInfoUI.SetText(SelectedStructure.Health);
                                    HighlightedEntity = SelectedStructure;
                                    HighlightedEntity.HighLight(true);

                                    List<Tile> possibleAttackTiles = new List<Tile>();
                                    if (SelectedStructure.CanAttackInTurn)
                                    {
                                        possibleAttackTiles.AddRange(TileGrid.GetTilesWithinAttackRange(clickedTile, SelectedStructure.AttackRange));
                                        foreach (Tile tile in possibleAttackTiles)
                                        {
                                            if (tile.ArmyEntityOnTile != null && tile.ArmyEntityOnTile.Player != PlayerManager.PlayerInTurn)
                                            {
                                                tile.HighLight(true, Color.red);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (clickedTile.ArmyEntityOnTile.Player != PlayerManager.PlayerInTurn && SelectedUnit == null && SelectedStructure == null && BuildFactory == null)
                                {
                                    Unit unit = (Unit)clickedTile.ArmyEntityOnTile;
                                    if (unit.Health > 0)
                                    {
                                        ResetHighlight();
                                        HighlightedEntity = unit;
                                        HighlightedEntity.HighLight(true);
                                        SelectionInfoUI.SetText(unit.Health, unit.Damage);
                                    }
                                }
                                else if(clickedTile.ArmyEntityOnTile.Player == PlayerManager.PlayerInTurn)
                                {
                                    ResetSelection();
                                    SelectionType = SelectionType.Unit;
                                    SelectedUnit = (Unit)clickedTile.ArmyEntityOnTile;
                                    ChangeScreenHandler.SetAllScreensInactive();
                                    SelectionInfoUI.SetText(SelectedUnit.Health, SelectedUnit.Damage);
                                    HighlightedEntity = SelectedUnit;
                                    HighlightedEntity.HighLight(true);

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
                            }
                        }
                        else if (clickedTile.ResourceNodeOnTile == null)
                        {
                            if (SelectionType == SelectionType.Build)
                            {
                                if (BuildFactory.GetComponent<StructureFactory>() != null)
                                {
                                    List<Tile> buildableTiles = TileGrid.GetAllBuildableTiles(3);
                                    if (buildableTiles.Contains(clickedTile) && PlayerManager.PlayerInTurn.Resources.RemoveResources(BuildFactory.GetComponent<StructureFactory>().Structure.GetComponent<Structure>().Price))
                                    {
                                        BuildFactory.GetComponent<StructureFactory>().InstantiateStructure(clickedTile);
                                    }
                                    ResetSelection(SelectionType.Structure);
                                }
                                else if (BuildFactory.GetComponent<UnitFactory>() != null)
                                {
                                    List<Tile> spawnableTiles = TileGrid.GetStructureSpawnableTiles(SelectedStructure);
                                    if (spawnableTiles.Contains(clickedTile) && PlayerManager.PlayerInTurn.Resources.RemoveResources(BuildFactory.GetComponent<UnitFactory>().unit.GetComponent<Unit>().Price))
                                    {
                                        BuildFactory.GetComponent<UnitFactory>().InstantiateUnit(clickedTile);
                                        clickedTile.HighLight(false);
                                    }
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
    
    public void SelectBuild(GameObject factory)
    {
        SelectionInfoUI.SetText();
        SelectionType = SelectionType.Build;
        BuildFactory = factory;
        SelectedUnit = null;

        if (BuildFactory.GetComponent<StructureFactory>() != null)
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
        SelectionInfoUI.HideScreen();
        TileGrid.TurnAllHighlightOf();
        SelectionType = selectionType;
        SelectedUnit = null;
        SelectedStructure = null;
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
