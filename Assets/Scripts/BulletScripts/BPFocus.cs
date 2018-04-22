using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPFocus : MonoBehaviour {

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
            for(int i = 0; i < 3; i++)
            {
                float angle = -30 + i * 30;
                GameObject obj = BulletPoolManager.Instance.enemyPool.GetBullet();
                if (obj != null)
                {
                    Bullet b = obj.GetComponent<Bullet>();
                    if (b != null)
                    {

                        b.moveDir = Quaternion.Euler(0, 0, angle) * transform.forward;
                        b.speed = 0.3f;
                        b.transform.position = transform.position;
                        obj.layer = LayerMask.NameToLayer("EnemyBullet");
                        obj.SetActive(true);
                    }
                }
            }

            yield return new WaitForSeconds(0.3f);
            
        }
    }
}
