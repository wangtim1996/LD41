using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class ScreenControl : MonoBehaviour {

    public float moveSection = 0.05f;
    public float scrollSpeed = 1.0f;

    public BoxCollider2D bounds;

	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Bounds b = GetCameraBounds();
        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;

        }
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Lock Mouse");
            Cursor.lockState = CursorLockMode.Confined;
        }

        if(!(b.min.x < bounds.bounds.min.x && b.max.x <= bounds.bounds.max.x) )
        {
            if (Input.mousePosition.x < Screen.width * moveSection)
            {
                //scroll left
                Camera.main.transform.position -= new Vector3(scrollSpeed, 0, 0) * Time.deltaTime;
            }
        }
        if(!(b.max.x > bounds.bounds.max.x && b.min.x >= bounds.bounds.min.x))
        {
            if (Input.mousePosition.x > Screen.width * (1 - moveSection))
            {
                //scroll right
                Camera.main.transform.position += new Vector3(scrollSpeed, 0, 0) * Time.deltaTime;
            }

        }
          
        if(!(b.min.y < bounds.bounds.min.y && b.max.y <= bounds.bounds.max.y))
        {
            if (Input.mousePosition.y < Screen.height * moveSection)
            {
                //scroll down
                Camera.main.transform.position -= new Vector3(0, scrollSpeed, 0) * Time.deltaTime;
            }
        }
        if (!(b.max.y > bounds.bounds.max.y && b.min.y >= bounds.bounds.min.y))
        {
            if (Input.mousePosition.y > Screen.height * (1 - moveSection))
            {
                //scroll up
                Camera.main.transform.position += new Vector3(0, scrollSpeed, 0) * Time.deltaTime;
            }

        }
        

        //if(b.min.x < bounds.bounds.min.x && b.max.x <= bounds.bounds.max.x)
        //{
        //    Camera.main.transform.position += new Vector3(bounds.bounds.min.x - b.min.x, 0, 0);
        //}
        //else if (b.max.x > bounds.bounds.max.x && b.min.x >= bounds.bounds.min.x)
        //{
        //    Camera.main.transform.position += new Vector3(bounds.bounds.max.x - b.max.x, 0, 0);
        //}

        //if (b.min.y < bounds.bounds.min.y && b.max.y <= bounds.bounds.max.y)
        //{
        //    Camera.main.transform.position += new Vector3(0, bounds.bounds.min.y - b.min.y, 0);
        //}
        //else if (b.max.y > bounds.bounds.max.y && b.min.y >= bounds.bounds.min.y)
        //{

        //    Camera.main.transform.position += new Vector3(0, bounds.bounds.max.y - b.max.y, 0);
        //}


    }

    Bounds GetCameraBounds()
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        float camHeight = Camera.main.orthographicSize * 2;
        Bounds b = new Bounds(Camera.main.transform.position, new Vector3(camHeight * aspectRatio, camHeight, 0));
        return b;
    }
    
}
