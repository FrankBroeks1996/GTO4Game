using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour {

    public void Play()
    {
        SceneManager.LoadScene("PlayerCustomizeScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
