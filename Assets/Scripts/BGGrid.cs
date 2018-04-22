using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGGrid : MonoBehaviour {

    int numLines = 10;
    public Collider2D mapBounds;
    public Material lineMat;
    public float width;

	// Use this for initialization
	void Start () {
        for(int i = 0; i <= numLines; i++)
        {
            float actualWidth = width;
            if(i == 0 || i == numLines)
            {
                actualWidth = 5 * width;
            }
            GameObject lineObj = new GameObject("line");
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.useWorldSpace = true;
            lr.material = lineMat;
            lr.startWidth = actualWidth;
            lr.endWidth = actualWidth;
            Vector3[] positions = new Vector3[2];
            positions[0] = new Vector3(mapBounds.bounds.min.x + i * mapBounds.bounds.extents.x * 2 / numLines, mapBounds.bounds.min.y, 20);
            positions[1] = new Vector3(mapBounds.bounds.min.x + i * mapBounds.bounds.extents.x * 2 / numLines, mapBounds.bounds.max.y, 20);
            lr.positionCount = positions.Length;
            lr.SetPositions(positions);
        }

        for (int i = 0; i <= numLines; i++)
        {
            float actualWidth = width;
            if (i == 0 || i == numLines)
            {
                actualWidth = 5 * width;
            }
            GameObject lineObj = new GameObject("line");
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.useWorldSpace = true;
            lr.material = lineMat;
            lr.startWidth = actualWidth;
            lr.endWidth = actualWidth;
            Vector3[] positions = new Vector3[2];
            positions[0] = new Vector3(mapBounds.bounds.min.x, mapBounds.bounds.min.y + i * mapBounds.bounds.extents.y * 2 / numLines, 20);
            positions[1] = new Vector3(mapBounds.bounds.max.x, mapBounds.bounds.min.y + i * mapBounds.bounds.extents.y * 2 / numLines, 20);
            lr.positionCount = positions.Length;
            lr.SetPositions(positions);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
