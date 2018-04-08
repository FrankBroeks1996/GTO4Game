using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneManager : MonoBehaviour {

    public static CrossSceneManager Manager;

    public string Winner;
    public List<string> PlayerNames;

    void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if(Manager == null)
        {
            DontDestroyOnLoad(transform.root.gameObject);
            Manager = this;
        }
    }

    public void SetPlayerNames(List<string> players)
    {
        PlayerNames = players;
    }
}
