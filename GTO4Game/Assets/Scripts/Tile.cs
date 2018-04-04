using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    
    public float x;
    public float y;
    public Unit UnitOnTile;
    public Structure StructureOnTile;
    public ResourceNode ResourceNodeOnTile;
    public bool Occupied = false;
    public bool IsHighLight = false;

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
}
