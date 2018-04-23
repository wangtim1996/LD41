using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

    enum State { IDLE, MOVE, ATTACKMOVE, ATTACKTARGET, EVADE };

    public float speed = 0.1f;
    private float maxSpeed = 1.0f;

    private Vector3 moveTarget;
    private Vector3 attackTarget;
    private GameObject enemyTarget;

    private State currState;

    public float shotCooldown = 0.5f;
    public float focusCooldown = 0.3f;
    private float currCooldown = 0.0f;

    public float accuracy = 0.5f;
    public float crouchSpeedModifier = 0.2f;

    public float range = 6.0f;
    public Collider2D[] enemiesInRange;

    private int enemyLayer;

    public GameObject highlight;


	// Use this for initialization
	void Start () {
        currState = State.IDLE;
        enemiesInRange = new Collider2D[20];
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        highlight.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
        currCooldown -= Time.deltaTime;
        if (currCooldown < 0) currCooldown = 0;
        switch (currState)
        {
            case State.IDLE:
                break;
            case State.MOVE:
                
                GameObject target = FindClosestEnemyInRange();
                if (target != null)
                {
                    if (currCooldown <= 0)
                    {
                        GameObject obj = BulletPoolManager.Instance.playerPool.GetBullet();
                        if (obj != null)
                        {
                            Bullet b = obj.GetComponent<Bullet>();
                            if (b != null)
                            {
                                Vector3 noiseTarget = new Vector3(Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy), 0);
                                noiseTarget += target.transform.position;

                                RotateToward(target.transform.position);
                                b.moveDir = Vector3.Normalize(noiseTarget - transform.position);
                                b.speed = 0.4f;
                                b.transform.position = transform.position;
                                obj.layer = LayerMask.NameToLayer("PlayerBullet");
                                obj.SetActive(true);
                                currCooldown = shotCooldown;
                            }
                        }
                    }

                }
                Move(moveTarget, speed);
                break;
            case State.ATTACKMOVE:
                GameObject target2 = FindClosestEnemyInRange();
                if(target2 != null)
                {
                    enemyTarget = target2;
                    Attack(target2.transform.position);

                }
                else
                {
                    Move(attackTarget, speed);
                }

                
                break;
            case State.ATTACKTARGET:
                if(enemyTarget != null)
                {
                    GameObject target3 = enemyTarget;
                    Attack(target3.transform.position);

                }
                break;
            case State.EVADE:
                break;
            default:
                break;
        }

	}

    public void MoveTo(Vector3 moveTarget)
    {
        this.enemyTarget = null;
        this.attackTarget = moveTarget;
        this.moveTarget = moveTarget;
        currState = State.MOVE;
    }

    public void AttackPos(Vector3 atkTarget)
    {

        this.enemyTarget = null;
        this.attackTarget = atkTarget;
        this.moveTarget = atkTarget;
        currState = State.ATTACKMOVE;
    }

    public void AttackEnemy(GameObject enemy)
    {
        currState = State.ATTACKTARGET;
        this.attackTarget = enemy.transform.position;
        this.moveTarget = enemy.transform.position;
        this.enemyTarget = enemy;

        currState = State.ATTACKMOVE;
    }

    private GameObject FindClosestEnemyInRange()
    {
        int numEnemies = Physics2D.OverlapCircleNonAlloc(transform.position, range, enemiesInRange, enemyLayer);
        float minDist = Mathf.Infinity;
        GameObject target = null;
        for(int i = 0; i < numEnemies; i++)
        {
            float currDist = Vector3.SqrMagnitude(enemiesInRange[i].transform.position - transform.position);
            if (currDist < minDist)
            {
                minDist = currDist;
                target = enemiesInRange[i].gameObject;
            }
        }
        return target;
    }

    private void Move(Vector3 target, float speed)
    {
        if (Vector3.Distance(transform.position, target) > 0.1)
        {
            RotateToward(target);
            Vector3 moveTarget = Vector3.Lerp(transform.position, target, speed);
            Vector3 translation = Vector3.ClampMagnitude(moveTarget - transform.position, maxSpeed);
            transform.position +=translation;
        }
    }

    void Attack(Vector3 target)
    {
        if (Vector3.Distance(target, transform.position) < range)
        {
            if (currCooldown <= 0)
            {
                GameObject obj = BulletPoolManager.Instance.playerPool.GetBullet();
                if (obj != null)
                {
                    Bullet b = obj.GetComponent<Bullet>();
                    if (b != null)
                    {
                        Vector3 noiseTarget = new Vector3(Random.Range(-accuracy, accuracy), Random.Range(-accuracy, accuracy), 0);
                        noiseTarget += target;
                        RotateToward(target);
                        b.moveDir = Vector3.Normalize(noiseTarget - transform.position);
                        b.speed = 0.4f;
                        b.transform.position = transform.position;
                        obj.layer = LayerMask.NameToLayer("PlayerBullet");
                        obj.SetActive(true);
                        currCooldown = focusCooldown;
                    }
                }
            }
        }
    }

    public void Stop()
    {
        AttackPos(transform.position);
    }

    public void Select()
    {
        if(highlight != null)
            highlight.SetActive(true);
    }
    public void Deselect()
    {
        if (highlight != null)
            highlight.SetActive(false);
    }

    private void RotateToward(Vector3 target)
    {
        float angle =  Mathf.Atan((target.y - transform.position.y) / (target.x - transform.position.x));
        float offset = 90;
        if(target.x - transform.position.x > 0)
        {
            offset = 270;
        }
        transform.rotation = Quaternion.Euler(0, 0, angle * 180 / Mathf.PI + offset);

    }

    private void OnDestroy()
    {
        GameManager.Instance.ShipDestroyed();
    }
}
