using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCustomizeUI : MonoBehaviour {

    [Header("Player name input fields")]
    public InputField inputFieldPlayer1;
    public InputField inputFieldPlayer2;

    public void StartGame()
    {
        if (inputFieldPlayer1.text != "" && inputFieldPlayer2.text != "")
        {
            List<string> playerNames = new List<string>();
            playerNames.Add(inputFieldPlayer1.text);
            playerNames.Add(inputFieldPlayer2.text);
            CrossSceneManager.Manager.SetPlayerNames(playerNames);
            SceneManager.LoadScene("GameScene");
        }
    }
}
