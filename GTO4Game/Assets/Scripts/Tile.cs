using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    
    public float x;
    public float y;
    public Unit ArmyEntityOnTile;
    public ResourceNode ResourceNodeOnTile;
    public bool Occupied = false;
    public bool IsHighLight = false;

    //Needed for A* pathfinding
    public int FScore = 0;
    public int HScore = 0;
    public int GScore = 0;
    public List<Tile> Neighbours;
    public Tile Previous;

	// Use this for initialization
	void Start () {
        x = transform.position.x;
        y = transform.position.z;
	}

    public void HighLight(bool highlightOn, Color? color = null)
    {
        if(color == null)
        {
            color = Color.yellow;
        }
        IsHighLight = !IsHighLight;
        if (highlightOn)
        {
            GetComponent<Renderer>().material.color = (Color)color;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void ResetTilePathFinding()
    {
        FScore = 0;
        HScore = 0;
        GScore = 0;
        Previous = null;
    }
}
