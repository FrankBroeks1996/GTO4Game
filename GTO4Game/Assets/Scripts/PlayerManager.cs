using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
    [Header("Game handlers")]
    public PathFinding PathFinding;
    public SelectionHandler SelectionHandler;
    public ChangeScreen ChangeScreen;

    [Header("Player information")]
    public List<Player> Players;
    public GameObject PlayerColorPanel;
    public Player PlayerInTurn;
    public Text PlayerText;

    [Header("Tile grid")]
    public Grid TileGrid;

	// Use this for initialization
	void Start () {
        PlayerInTurn = Players[0];
        Players[0].gameObject.SetActive(true);
        PlayerColorPanel.GetComponent<Image>().color = PlayerInTurn.color;
        SetPlayerNames();
        PathFinding.HandlePlayerHarvesters(PlayerInTurn);
        PlayerText.text = PlayerInTurn.Name;
    }

    public void InitializeBaseListeners(List<Structure> structures)
    {
        foreach (Structure structure in structures)
        {
            structure.DeathEvent.AddListener(EndGame);
        }
    }

    public void SetPlayerNames()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].Name = CrossSceneManager.Manager.PlayerNames[i];
        }
    }

    public void EndGame()
    {
        CrossSceneManager.Manager.Winner = PlayerInTurn.Name;
        SceneManager.LoadScene("GameOverScene");
    }

    public void ChangePlayer()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].gameObject.SetActive(false);
            if (PlayerInTurn == Players[i])
            {
                if(i == Players.Count - 1)
                {
                    PlayerInTurn = Players[0];
                }
                else
                {
                    PlayerInTurn = Players[i + 1];
                }
                break;
            }
        }
        PlayerColorPanel.GetComponent<Image>().color = PlayerInTurn.color;
        PlayerInTurn.gameObject.SetActive(true);
        ChangeScreen.ChangePlayerScreen(PlayerInTurn);
        ResetPlayerUnits();
        SelectionHandler.ResetSelection();
        PathFinding.HandlePlayerHarvesters(PlayerInTurn);
        PlayerText.text = PlayerInTurn.Name;
    }

    public void ResetPlayerUnits()
    {
        List<Tile> tilesWithUnit = TileGrid.GetAllTilesWithPlayerUnit(PlayerInTurn);
        foreach (Tile tile in tilesWithUnit)
        {
            tile.ArmyEntityOnTile.CanMoveInTurn = true;
            tile.ArmyEntityOnTile.CanAttackInTurn = true;
        }
    }
}
