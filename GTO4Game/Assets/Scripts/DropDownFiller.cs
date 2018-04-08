using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class DropDownFiller : MonoBehaviour {

    private Dropdown dropdown;

    void Awake()
    {
        dropdown = GetComponent<Dropdown>();
    }

	// Use this for initialization
	void Start () {
        //dropdown.options.Clear();
        AddDropDownItem(Color.blue);
	}
	
	public void AddDropDownItem(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        Dropdown.OptionData item = new Dropdown.OptionData(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0)));
        dropdown.options.Add(item);
    }
}
