using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour {

	public void CloseGame()
    {
        Application.Quit();
    }

    public void SwitchState()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
