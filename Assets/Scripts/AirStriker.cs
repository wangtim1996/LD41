using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirStriker : MonoBehaviour {

    enum State { WAIT, TARGET ,ATTACK};


    public GameObject AirStrikePattern;
    private GameObject currPattern;
    private Collider2D[] shipsInRange;
    private int shipLayer;
    private State currState;
    private GameObject target;

    private float targettingTime = 1.5f;
    public GameObject reticule;

    // Use this for initialization
    void Start () {
        reticule.SetActive(false);
        currState = State.WAIT;
        shipsInRange = new Collider2D[10];
        shipLayer = 1 << LayerMask.NameToLayer("Ship");
        StartCoroutine("AI");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        if(currState == State.TARGET && target != null)
        {
            reticule.transform.Rotate(0, 0, Time.deltaTime * 100);
            reticule.transform.position = target.transform.position;
        }
    }

    IEnumerator AI()
    {
        yield return new WaitForSeconds(1.0f + Random.Range(0, 1));
        while (true)
        {
            currState = State.WAIT;
            target = null;
            while ((target = TargetRandomShipInRange(99)) == null)
            {
                yield return new WaitForSeconds(0.5f);
            }
            if (target != null)
            {
                reticule.SetActive(true);
                currState = State.TARGET;
                yield return new WaitForSeconds(targettingTime);

                reticule.SetActive(false);

                if(target != null)
                {
                    currPattern = Instantiate(AirStrikePattern, target.transform.position, Quaternion.identity);

                }

            }

            currState = State.WAIT;
            yield return new WaitForSeconds(3.0f + Random.Range(0, 1));
        }
    }

    GameObject TargetRandomShipInRange(float range)
    {
        int numShips = Physics2D.OverlapCircleNonAlloc(transform.position, range, shipsInRange, shipLayer);
        GameObject target = null;
        if (numShips != 0)
        {
            int index = Random.Range(0, numShips);
            target = shipsInRange[index].gameObject;
        }
        return target;
    }
}
