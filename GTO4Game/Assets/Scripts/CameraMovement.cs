using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float Speed = 15;
    public float ScrollSpeed = 300;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(Speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-Speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -Speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, Speed * Time.deltaTime, 0));
        }
        if(Input.mouseScrollDelta.y > 0)
        {
            transform.Translate(new Vector3(0, 0, ScrollSpeed * Time.deltaTime));
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            if(transform.position.y < 50)
            {
                transform.Translate(new Vector3(0, 0, -ScrollSpeed * Time.deltaTime));
            }
        }

        CheckBounds();
    }

    public void CheckBounds()
    {
        if(transform.position.y < 8)
        {
            transform.position = new Vector3(transform.position.x, 8, transform.position.z);
        }
    }
}
