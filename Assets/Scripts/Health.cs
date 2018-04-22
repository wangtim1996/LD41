using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public int health = 100;
    public int currHealth;

    public AudioClip hitSound;
    public AudioClip explosionSound;


    private bool invincible = false;
    public bool player = false;
    public float invincibleTime = 0.5f;

    public GameObject model;
    

	// Use this for initialization
	void Start () {
        currHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet b = collision.gameObject.GetComponent<Bullet>();
        if (!invincible && b != null)
        {

            AudioManager.Instance.PlayClip(hitSound);
            health -= b.damage;

            if (player)
            {
                StartCoroutine("PlayerHit");
            }
            if(b.destroyOnHit)
            {
                b.gameObject.SetActive(false);
            }
        }

        checkHealth();
    }

    void checkHealth()
    {
        if(health <= 0)
        {
            AudioManager.Instance.PlayClip(explosionSound);
            Destroy(gameObject);
        }
    }

    IEnumerator PlayerHit()
    {
        if (model != null)
        {
            invincible = true;
            bool render = true;
            for (int i = 0; i < 5; i++)
            {

                if (render)
                {
                    model.SetActive(true);
                }
                else
                {
                    model.SetActive(false);
                }
                render = !render;
                yield return new WaitForSeconds(invincibleTime / 5);
            }
            render = true;
            invincible = false;
        }
        
    }
}
