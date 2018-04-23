using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserSnake : MonoBehaviour {

    enum State { LOOKING, AIMING, TARGETING,  SHOOTING, WIGGLE };

    bool doneStart = false;

    public GameObject bodySegmentPrefab;
    public int numBodySegments = 5;
    List<GameObject> bodySegments;
    public GameObject head;

    private int NUMTICKS = 50;
    private float EMERGE_TIME = 1.0f;


    private Collider2D[] shipsInRange;
    private int shipLayer;

    public GameObject laserAim;
    public GameObject laserShot;
    State currState;
    float timeStart;
    GameObject target;
    private const float AIMTIME = 1;

    private const float TARGETTIME = 1;
    private const float SHOOTTIME = 1.5f;

    Vector3 p0;
    Vector3 p1;
    Quaternion q0;
    Quaternion q1;

    Vector3 savedPos;
    Quaternion savedQuat;

    private bool dead = false;

    public AudioClip segmentDestroySfx;
    public AudioClip laserTargetSfx;
    public AudioClip laserShootSfx;

    // Use this for initialization
    void Start () {
        bodySegments = new List<GameObject>();
        for(int i = 0; i < numBodySegments; i++)
        {
            GameObject obj = Instantiate(bodySegmentPrefab, transform.parent.position, transform.parent.rotation, transform);
            bodySegments.Add(obj);
        }


        shipsInRange = new Collider2D[10];
        shipLayer = 1 << LayerMask.NameToLayer("Ship");
        StartCoroutine("Emerge");

	}
	
	// Update is called once per frame
	void Update () {
        if (!doneStart) return;
        if(head == null)
        {
            if(!dead)
            {
                dead = true;
                StopAllCoroutines();
                Debug.Log("Start Destruction");
                StartCoroutine("Destruction");

            }
        }
        
	}

    private void LateUpdate()
    {
        if (!doneStart || dead) return;
        switch(currState)
        {
            case State.LOOKING:
                break;
            case State.AIMING:
                {

                    float t = (Time.time - timeStart) / AIMTIME;
                    Debug.Log(t);

                    transform.position = Vector3.Lerp(p0, p1, t);
                    transform.rotation = Quaternion.Slerp(q0, q1, t);

                    Vector3 ap0 = transform.parent.position;
                    Vector3 ap1 = ap0 + transform.parent.forward * 3.0f;
                    Vector3 ap3 = transform.position;
                    Vector3 ap2 = ap3 - transform.forward * 1.0f;

                    float bt = 1.0f;
                    float indivT = bt;
                    BezierSet(head, indivT, ap0, ap1, ap2, ap3);
                    foreach (GameObject go in bodySegments)
                    {
                        indivT -= 1.0f / numBodySegments;
                        BezierSet(go, indivT, ap0, ap1, ap2, ap3);
                    }

                    savedPos = transform.position;
                    savedQuat = transform.rotation;

                }

                break;
            case State.TARGETING:
            case State.SHOOTING:
                {
                    transform.position = savedPos;
                    transform.rotation = savedQuat;
                    Vector3 ap0 = transform.parent.position;
                    Vector3 ap1 = ap0 + transform.parent.forward * 3.0f;
                    Vector3 ap3 = transform.position;
                    Vector3 ap2 = ap3 - transform.forward * 1.0f;

                    float bt = 1.0f;
                    float indivT = bt;
                    BezierSet(head, indivT, ap0, ap1, ap2, ap3);
                    foreach (GameObject go in bodySegments)
                    {
                        indivT -= 1.0f / numBodySegments;
                        BezierSet(go, indivT, ap0, ap1, ap2, ap3);
                    }
                }
                break;
            case State.WIGGLE:
                break;
            default:
                break;
        }

    }

    IEnumerator Emerge()
    {
        


        for (int i = 0; i < NUMTICKS; i++)
        {
            Vector3 p0 = transform.parent.position;
            Vector3 p1 = p0 + transform.parent.forward * 3.0f;

            Vector3 p3 = transform.position;
            Vector3 p2 = p3 - transform.forward * 1.0f;


            float t = (float)i / NUMTICKS;
            float indivT = t;
            BezierSet(head, indivT, p0, p1, p2, p3);
            foreach(GameObject go in bodySegments)
            {
                indivT -= 1.0f / numBodySegments;
                BezierSet(go, indivT, p0, p1, p2, p3);
            }
            yield return new WaitForSeconds(EMERGE_TIME/NUMTICKS);
        }
        doneStart = true;

        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f + Random.Range(0, 0.5f));
        while (true)
        {
            target = null;
            while ((target = TargetRandomShipInRange(99)) == null)
            {
                yield return new WaitForSeconds(0.5f);
            }
            if (target != null)
            {
                Debug.Log("AIMING");
                //Aim

                p0 = head.transform.position;
                p1 = transform.parent.position + transform.parent.forward + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
                q0 = head.transform.rotation;
                q1 = Quaternion.LookRotation(target.transform.position - p1, Vector3.forward);
                timeStart = Time.time;
                currState = State.AIMING;
                yield return new WaitForSeconds(AIMTIME);


                //show aim
                AudioManager.Instance.PlayClip(laserTargetSfx);
                laserAim.SetActive(true);
                currState = State.TARGETING;
                yield return new WaitForSeconds(TARGETTIME);
                laserAim.SetActive(false);


                AudioManager.Instance.PlayClip(laserShootSfx);
                laserShot.SetActive(true);
                currState = State.SHOOTING;
                yield return new WaitForSeconds(SHOOTTIME);
                laserShot.SetActive(false);


                //fire

            }
            //Wiggle-wiggle
            yield return new WaitForSeconds(0.1f + Random.Range(0, 0.5f));

        }
        
    }

    GameObject TargetRandomShipInRange(float range)
    {
        int numShips = Physics2D.OverlapCircleNonAlloc(head.transform.position, range, shipsInRange, shipLayer);
        GameObject target = null;
        if (numShips != 0)
        {
            int index = Random.Range(0, numShips);
            target = shipsInRange[index].gameObject;
        }
        return target;
    }

    void BezierSet(GameObject go, float t , Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        if (!doneStart && t < 0) return;

        float u = 1 - t;
        float tt = t * t;
        float ttt = tt * t;
        float uu = u * u;
        float uuu = uu * u;

        Vector3 pos = uuu * p0 + 3 * uu * t * p1 + 3 * u * tt * p1 + ttt * p3;
        Vector3 dir = -3 * uu * p0 + (3 * uu - 6 * u * t) * p1 + (6 * u * t - 3 * tt) * p2 + 3 * tt * p3;

        go.transform.position = pos;
        go.transform.rotation = Quaternion.LookRotation(dir, Vector3.forward);
    }

    IEnumerator Destruction()
    {
        foreach(GameObject sg in bodySegments)
        {
            AudioManager.Instance.PlayClip(segmentDestroySfx);
            Destroy(sg);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(transform.parent.gameObject);
    }
}
