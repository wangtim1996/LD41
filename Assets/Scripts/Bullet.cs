using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Vector3 moveDir;
    public float speed = 0.1f;
    public int damage = 1;
    public bool destroyOnHit = true;
    public bool checkBounds = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position += moveDir * speed;
        Bounds b = BulletPoolManager.Instance.mapBounds.bounds;
        if (checkBounds && (transform.position.x < b.min.x || transform.position.x > b.max.x || transform.position.y < b.min.y || transform.position.y > b.max.y))
        {
            gameObject.SetActive(false);
        }
	}
}
