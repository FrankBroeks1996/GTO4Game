﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScreen : MonoBehaviour {

    public List<PlayerUI> AllPlayerScreens;

	public void SwitchToBuildStructureScreen(PlayerUI playerUI)
    {
        SetAllScreensInactive();
        playerUI.BuildScreen.SetActive(true);
    }

    public void SwitchToCreateUnitScreen(Structure structure)
    {
        SetAllScreensInactive();
        if (structure.UnitsScreen != null)
        {
            structure.UnitsScreen.SetActive(true);
        }
    }

    public void SetAllScreensInactive()
    {
        foreach (PlayerUI playerUI in AllPlayerScreens)
        {
            playerUI.BuildScreen.SetActive(false);
            foreach (GameObject screen in playerUI.UnitScreens)
            {
                screen.SetActive(false);
            }
        }
    }

    public void ChangePlayerScreen(Player player)
    {
        SetAllScreensInactive();
        foreach(PlayerUI playerUI in AllPlayerScreens)
        {
            playerUI.gameObject.SetActive(false);
        }
        player.PlayerUI.gameObject.SetActive(true);
    }
}
