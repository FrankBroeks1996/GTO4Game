using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    public List<Player> Players;
    public GameObject PlayerColorPanel;
    public Player PlayerInTurn;


	// Use this for initialization
	void Start () {
        PlayerInTurn = Players[0];
        Players[0].gameObject.SetActive(true);
        PlayerColorPanel.GetComponent<Image>().color = PlayerInTurn.color;
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
    }
}
