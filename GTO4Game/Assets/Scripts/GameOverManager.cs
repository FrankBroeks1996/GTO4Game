using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour {

    public static GameOverManager Manager;

    public string Winner;
    public List<PlayerCustomization> PlayerCustomizations;

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
}

[Serializable]
public struct PlayerCustomization
{
    public string Name;
    public Color Color;
}
