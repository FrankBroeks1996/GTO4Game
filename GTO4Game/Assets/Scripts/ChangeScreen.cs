using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScreen : MonoBehaviour {
    
    public GameObject BuildScreen;
    public GameObject UnitControlScreen;
    public List<GameObject> UnitScreens;

	public void SwitchToBuildStructureScreen()
    {
        SetAllScreensInactive();
        BuildScreen.SetActive(true);
    }

    public void SwitchToCreateUnitScreen(Structure structure)
    {
        SetAllScreensInactive();
        if (structure.UnitsScreen != null)
        {
            structure.UnitsScreen.SetActive(true);
        }
    }

    public void SwitchToUnitControlScreen()
    {
        SetAllScreensInactive();
        UnitControlScreen.SetActive(true);
    }

    public void SetAllScreensInactive()
    {
        BuildScreen.SetActive(false);
        foreach (GameObject screen in UnitScreens)
        {
            screen.SetActive(false);
        }
    }
}
