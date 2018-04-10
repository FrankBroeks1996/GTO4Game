using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

    [Header("Text that displays the winner")]
    public Text WinnerText;

    private string winner;

	// Use this for initialization
	void Start () {
        winner = CrossSceneManager.Manager.Winner;
        WinnerText.text = winner + " Won the game!!!";
	}

    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }
}
