using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPEightWay : MonoBehaviour {

	// Use this for initialization
	void Start () {

        StartCoroutine("pattern");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator pattern()
    {
        while(true)
        {
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 2 * Mathf.PI / 8 + Random.Range(0, 0.3f);
                Vector3 shotdir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                GameObject obj = BulletPoolManager.Instance.enemyPool.GetBullet();
                if (obj != null)
                {
                    Bullet b = obj.GetComponent<Bullet>();
                    if (b != null)
                    {
                        b.moveDir = shotdir;
                        b.speed = 0.3f;
                        b.transform.position = transform.position;
                        obj.layer = LayerMask.NameToLayer("EnemyBullet");
                        obj.SetActive(true);
                    }
                }

            }
            yield return new WaitForSeconds(0.5f);
        }
        
    }
}
