using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSceneManager : MonoBehaviour {

    [Header("Singleton object controlling the flow trough scenes")]
    public static CrossSceneManager Manager;

    [Header("Player information that needs to be persisted trough scenes")]
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
