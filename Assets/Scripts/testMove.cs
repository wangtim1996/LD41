using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float horz = Input.GetAxis("Horizontal") * 5.0f;
        float vert = Input.GetAxis("Vertical") * 5.0f;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position += new Vector3(horz, vert, 0);
        Vector3 dir = mousePos - transform.position;
        dir.z = 0;
        transform.rotation = Quaternion.LookRotation(dir, -Vector3.up);
    }
}
