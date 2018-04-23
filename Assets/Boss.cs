using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public bool gameStarted = false;
    public int numWeapons = 0;
    public List<Transform> weaponSlots;
    public GameObject shotgun;


	// Use this for initialization
	void Awake () {
	}
	
	// Update is called once per frame
	void Update () {
		if(gameStarted)
        {
            bool weaponAlive = false;
            foreach(Transform t in weaponSlots)
            {
                if(t.childCount > 0)
                {
                    weaponAlive = true;
                }
            }

            if (!weaponAlive)
            {
                //WIN
                Debug.Log("Win");
                GameManager.Instance.Win(true);
                gameStarted = false;
            }
        }
	}

    public void StartGame()
    {
        gameStarted = true;
    }

    public Transform GetWeaponSlot()
    {
        if(numWeapons >= 8)
        {
            return null;
        }
        return weaponSlots[numWeapons++];
        
    }
    
}
