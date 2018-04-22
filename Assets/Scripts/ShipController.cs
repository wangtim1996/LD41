using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    public GameObject selectionBox;
    private Vector3 startingPos;
    private Vector3 currPos;

    private List<Ship> selectedShips;


    private int enemyLayer;

    // Use this for initialization
    void Start () {
        selectionBox.SetActive(false);
        selectedShips = new List<Ship>();
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    }
	
	// Update is called once per frame
	void Update () {

        //SELECTING SHIPS
		if(Input.GetButtonDown("Fire1"))
        {
            foreach(Ship s in selectedShips)
            {
                s.Deselect();
            }
            selectedShips.Clear();
            startingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startingPos.z = 0;
            currPos = startingPos;
            selectionBox.transform.position = startingPos;
            selectionBox.SetActive(true);
        }

        if(selectionBox.activeInHierarchy)
        {
            currPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currPos.z = 0;
            Vector3 scale = currPos - startingPos;
            scale.z = 1;
            selectionBox.transform.localScale = scale;
        }

        if(Input.GetButtonUp("Fire1"))
        {
            Vector3 center = (startingPos + currPos) / 2;
            Vector3 halfways = (startingPos - currPos);
            halfways.x = Mathf.Abs(halfways.x);
            halfways.y = Mathf.Abs(halfways.y);
            halfways.z = 1;
            Collider2D[] colliders = Physics2D.OverlapBoxAll(center, halfways, 0, 1<<LayerMask.NameToLayer("Ship"));
            foreach(Collider2D coll in colliders)
            {
                if(coll.gameObject.GetComponent<Ship>())
                {
                    Ship s = coll.gameObject.GetComponent<Ship>();
                    selectedShips.Add(s);
                    s.Select();
                }
            }
            selectionBox.SetActive(false);
        }

        //CONTROLLING SHIPS
        currPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currPos.z = 0;
        if (Input.GetButton("Fire2"))
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(currPos, 1 << LayerMask.NameToLayer("Enemy"));
            GameObject target = null;
            foreach (Collider2D coll in colliders)
            {
                target = coll.gameObject;
                break;
            }


            if (target != null)
            {
                foreach (Ship ship in selectedShips)
                {
                    ship.AttackEnemy(target);
                }
            }
            else
            {
                foreach (Ship ship in selectedShips)
                {
                    ship.MoveTo(currPos);
                }

            }
        }

        if (Input.GetButtonDown("Attack"))
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(currPos, 1 << LayerMask.NameToLayer("Enemy"));
            GameObject target = null;
            foreach (Collider2D coll in colliders)
            {
                target = coll.gameObject;
                break;
            }

            
            if (target != null)
            {
                foreach (Ship ship in selectedShips)
                {
                    ship.AttackEnemy(target);
                }
            }
            else
            {
                foreach (Ship ship in selectedShips)
                {
                    ship.AttackPos(currPos);
                }

            }
        }
	}
}
