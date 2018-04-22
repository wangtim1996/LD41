using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour {

    public GameObject baseBullet;
    private List<GameObject> bulletList;
    public int poolSize = 300;

	// Use this for initialization
	void Awake () {
        bulletList = new List<GameObject>();
        for(int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(baseBullet, transform);
            bulletList.Add(bullet);

            bullet.SetActive(false);
        }
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if(!bulletList[i].activeInHierarchy)
            {
                return bulletList[i];
            }
            
        }

        return null;
    }
}
