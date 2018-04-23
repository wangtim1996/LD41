using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    enum EnemyState { EIGHTWAY, FOCUS };
    public float speed;

    public float shotCooldown = 1.0f;
    public float eightwayCooldown = 0.5f;
    private float currCooldown;

    private GameObject focusTarget;

    private float aiEightwayTimer = 3;
    private float aiTimer;

    private EnemyState currState;

    public GameObject target;
    public GameObject eightWayPattern;
    public GameObject focusPattern;

    private GameObject currPattern;

    private Collider2D[] shipsInRange;
    private int shipLayer;

    public GameObject targetPrefab;

    private GameObject targetObj;

	// Use this for initialization
	void Start () {
        currPattern = Instantiate(eightWayPattern, transform.position, Quaternion.identity);
        currState = EnemyState.EIGHTWAY;


        aiTimer = aiEightwayTimer;
        currCooldown = shotCooldown;


        shipsInRange = new Collider2D[10];
        shipLayer = 1 << LayerMask.NameToLayer("Ship");

	}
	
	// Update is called once per frame
	void Update () {

        currCooldown -= Time.deltaTime;
        aiTimer -= Time.deltaTime;

        //for aiming
        switch (currState)
        {
            case EnemyState.EIGHTWAY:
                break;
            case EnemyState.FOCUS:
                if(target != null)
                {
                    targetObj.transform.position = target.transform.position;
                    currPattern.transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
                }
                break;
            default:
                break;
        }

        if(aiTimer < 0)
        {
            aiTimer = aiEightwayTimer + +Random.Range(0, 1);
            switch (currState)
            {
                case EnemyState.EIGHTWAY:
                    TargetRandomShipInRange(30);
                    if(target != null)
                    {
                        Destroy(currPattern);
                        targetObj = Instantiate(targetPrefab, target.transform.position, Quaternion.identity, transform);
                        currPattern = Instantiate(focusPattern, transform.position, Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up), transform);
                        currState = EnemyState.FOCUS;
                    }
                    break;
                case EnemyState.FOCUS:
                    Destroy(currPattern);
                    Destroy(targetObj);
                    currPattern = Instantiate(eightWayPattern, transform.position, Quaternion.identity);
                    currState = EnemyState.EIGHTWAY;
                    break;
                default:
                    break;
            }
        }


        
	}

    void TargetRandomShipInRange(float range)
    {
        int numShips = Physics2D.OverlapCircleNonAlloc(transform.position, range, shipsInRange, shipLayer);
        target = null;
        if(numShips != 0)
        {
            int index = Random.Range(0, numShips);
            target = shipsInRange[index].gameObject;
        }
    }

    private void OnDestroy()
    {
        if(currPattern)
        {
            Destroy(currPattern);
        }
    }
}
