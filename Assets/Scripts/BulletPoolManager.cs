using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour {

    public static BulletPoolManager Instance;

    public BulletPool playerPool;
    public BulletPool enemyPool;
    public BoxCollider2D mapBounds;

	// Use this for initialization
	void Awake () {
		if(Instance != null)
        {
            Debug.LogError("BulletPoolManager singleton fail");
            return;
        }
        Instance = this;
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
