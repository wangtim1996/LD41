using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPAirStrike : MonoBehaviour {
    
    private int NumBullets = 12;
    private float spawnTime = 0.5f;
    private float holdTime = 0.5f;
    private float radius = 5.0f;
    private float moveAngle = 2.8f;
    // Use this for initialization
    void Start () {
        StartCoroutine("Pattern");
	}
	
	// Update is called once per frame
	void Update () {
	}

    IEnumerator Pattern()
    {
        float initAngle = Random.Range(0, 360);
        List<Bullet> bullets = new List<Bullet>();
        for (int i = 0; i < NumBullets; i++)
        {
            float spawnAngle = i * 2 * Mathf.PI / NumBullets + initAngle;
            Vector3 pos = new Vector3(Mathf.Cos(spawnAngle), Mathf.Sin(spawnAngle), 0) * radius;
            GameObject obj = BulletPoolManager.Instance.enemyPool.GetBullet();
            if (obj != null)
            {
                Bullet b = obj.GetComponent<Bullet>();
                if (b != null)
                {
                    b.moveDir = Vector3.zero;
                    b.speed = 0;
                    b.transform.position = transform.position + pos;
                    b.gameObject.SetActive(true);
                    bullets.Add(b);
                    Debug.Log("Spawned bullet at" + b.transform.position);
                }
            }
            else
            {
                bullets.Add(null);
            }
            yield return new WaitForSeconds(spawnTime / NumBullets);
        }
        yield return new WaitForSeconds(holdTime);

        for (int i = 0; i < NumBullets; i++)
        {
            Bullet b = bullets[i];
            if(b != null && b.gameObject.activeInHierarchy)
            {
                float angle = (i * 2 * Mathf.PI / NumBullets) + initAngle + moveAngle;
                b.moveDir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
                b.speed = 0.3f;

            }
        }
    }
}
