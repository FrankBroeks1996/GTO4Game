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
            if (Physics.Raycast(ray, out raycastHit, 100f, LayerMask.GetMask("Grid")))
            {
                Tile clickedTile = raycastHit.collider.GetComponent<Tile>();
                if (clickedTile != null)
                {
                    if (clickedTile.UnitOnTile != null && clickedTile.UnitOnTile.Player != null && clickedTile.UnitOnTile.Player == PlayerManager.PlayerInTurn)
                    {
                        ResetSelection();
                        SelectionType = SelectionType.Unit;
                        SelectedUnit = clickedTile.UnitOnTile;
                        ChangeScreenHandler.SetAllScreensInactive();
                        List<Tile> possibleTiles = TileGrid.GetTilesWithinMovementRange(clickedTile, SelectedUnit.MovementCount);
                        possibleTiles.AddRange(TileGrid.GetTilesWithinAttackRange(clickedTile, SelectedUnit.MovementCount));
                        foreach (Tile tile in possibleTiles)
                        {
                            if (tile.Occupied)
                            {
                                tile.HighLight(true, Color.red);
                            }
                            else
                            {
                                tile.HighLight(true);
                            }
                        }
                    }
                    else if(clickedTile.StructureOnTile != null && clickedTile.StructureOnTile.player == PlayerManager.PlayerInTurn)
                    {
                        ResetSelection();
                        SelectionType = SelectionType.Structure;
                        SelectedStructure = clickedTile.StructureOnTile;
                        ChangeScreenHandler.SwitchToCreateUnitScreen(SelectedStructure);
                    }
                    else if(clickedTile.ResourceNodeOnTile == null)
                    {
                        switch (SelectionType)
                        {
                            case SelectionType.Build:
                                if (BuildFactory.GetComponent<StructureFactory>() != null)
                                {
                                    BuildFactory.GetComponent<StructureFactory>().InstantiateStructure(clickedTile);
                                } else if (BuildFactory.GetComponent<UnitFactory>() != null)
                                {
                                    BuildFactory.GetComponent<UnitFactory>().InstantiateUnit(clickedTile);
                                }
                                break;
                            case SelectionType.Unit:
                                if (TileGrid.IsTileWithinRange(SelectedUnit.transform.parent.GetComponent<Tile>(), SelectedUnit.MovementCount, clickedTile))
                                {
                                    if (clickedTile.UnitOnTile == null && clickedTile.StructureOnTile == null)
                                    {
                                        MovementHandler.Move(SelectedUnit, clickedTile);
                                    }
                                    else
                                    {
                                        if (clickedTile.UnitOnTile != null)
                                        {
                                            MovementHandler.UnitAttackUnit(SelectedUnit.Damage, clickedTile.UnitOnTile.gameObject);
                                        }else if(clickedTile.StructureOnTile != null)
                                        {
                                            MovementHandler.UnitAttackUnit(SelectedUnit.Damage, clickedTile.StructureOnTile.gameObject);
                                        }
                                    }
                                    ResetSelection();
                                }
                                break;
                            case SelectionType.Structure:
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
    
    public void SelectBuild(GameObject factory)
    {
        SelectionType = SelectionType.Build;
        BuildFactory = factory;
        SelectedStructure = null;
        SelectedUnit = null;
    }

    public void ResetSelection()
    {
        TileGrid.TurnAllHighlightOf();
        SelectionType = SelectionType.None;
        SelectedUnit = null;
        SelectedStructure = null;
        BuildFactory = null;
        ChangeScreenHandler.SwitchToBuildStructureScreen();
    }
}
