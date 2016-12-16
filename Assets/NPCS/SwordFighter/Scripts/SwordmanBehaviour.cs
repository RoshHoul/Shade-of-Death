using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwordmanBehaviour : MonoBehaviour {

    [SerializeField]
    public Animator animator;

    public int health = 100;
    public Transform Enemy;
    public Transform[] Waypoints;
    public float Speed;
    public int curWayPoint;
    public bool doPatrol = true;
    public Vector3 Target;
    public Vector3 MoveDirection;
    public Vector3 Velocity;
    public List<Transform> visibleTargets;
    public int target;
    public Transform circlePosition;
    public GameObject hitCheck;
    public GameObject sword;
    public GameObject patrol;
    Rigidbody rb;

    public float curTime = 0f;
    private float pauseDuration = 3f;
    private int m_Speed = 2;
    private float randFloat = 0.0f;
    private int targetF;
    private int sizeList;
    private bool fightTime = false;

    bool hitFin;
    public bool inCombat;
    public bool canFight;
    public bool canCharge = false;


    enum AIState { Chase, EnterCombat, Strike, Dead, Idle }

    AIState current_state;
    private AIState last_state;

    // Use this for initialization
    void Start()
    {

        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        animator.SetInteger("Speed", 6);
        current_state = AIState.Idle;


    }

    void Update()
    {
        hitFin = sword.GetComponent<SwordCheck>().attackLanded;


        switch (current_state)
        {
            case AIState.Idle:
                Idle();
                break;
            case AIState.Chase:
                ReachTarget();
                break;
            case AIState.EnterCombat:
                EnterFight();
                break;
            case AIState.Strike:
                Hit();
                break;
            case AIState.Dead:
                Die();
                break;
        }



        if (health <= 0)
            current_state = AIState.Dead;
        Debug.Log(current_state);
        last_state = current_state;

//        Debug.Log(inCombat);

        inCombat = hitCheck.GetComponent<CharacterBattleLogic>().inCombat;
    }

    void Die()
    {
        animator.SetFloat("Speed", 0);
        MoveDirection = Target.normalized;
        Velocity = Vector3.zero;
        animator.SetInteger("Health", 0);

    }

    void Idle()
    {
        animator.SetFloat("Speed", 0);
        animator.SetInteger("Health", 100);
        //            Debug.Log("TUPANICI SHUTOVE") ;
        transform.LookAt(Target);
        MoveDirection = Target.normalized;
        Velocity = Vector3.zero;
        //        this.GetComponent<Rigidbody>().isKinematic = true;

        if (patrol.GetComponent<PatrolAgent>().targetFound == true)
            current_state = AIState.Chase;
    }

    void EnterFight()
    {
        
        Target = circlePosition.position;
        Target.y = transform.position.y;
        
        MoveDirection = Target - transform.position;
        Velocity = rb.velocity;
        animator.SetInteger("Speed", 1);
        Speed = 3;

        Velocity = MoveDirection.normalized * Speed;

        if (MoveDirection.magnitude < 1)
        {
            MoveDirection = Target.normalized;
            Velocity = Vector3.zero;
            animator.SetInteger("Speed", 0);
            curTime += Time.deltaTime;
            if (curTime >= pauseDuration)
            {
                if (inCombat == false)
                {
                    current_state = AIState.Strike;
                    curTime = 0f;
//                    Debug.Log("BLABLA" + current_state);
                }
            }
        }

        rb.velocity = Velocity;
        transform.LookAt(Enemy.position);
    }
    
    void Hit()
    {
        bool endAction = false;

        animator.SetFloat("Speed", 1);
        Target = Enemy.position;
        MoveDirection = Target - transform.position;
        Velocity = rb.velocity;
        Speed = 3;

        Velocity = MoveDirection.normalized * Speed;
        rb.velocity = Velocity;

        if (inCombat)
        {
            animator.SetFloat("Speed", 0);
            Debug.Log("TUPANICI SHUTOVE") ;
            transform.LookAt(Target);
            MoveDirection = Target.normalized;
            Velocity = Vector3.zero;
            animator.SetBool("canHit", true);
            
        }

        curTime += Time.deltaTime;
        if (curTime > pauseDuration)
//            animator.SetBool("canHit", false);

//        Debug.Log("End Action is " + endAction + "HitFin is " + hitFin);

        if ((hitFin))
        {
            animator.SetBool("canHit", false);
            current_state = AIState.EnterCombat;
        }
//        Debug.Log("CURRCHECKBEFOREIF" + current_state);
//        inCombat = false;
    }

    void ReachTarget()
    {
        Target = Enemy.position;
        MoveDirection = Target - transform.position;
        Velocity = rb.velocity;
        animator.SetInteger("Speed", 2);
        Speed = 6;

        Velocity = MoveDirection.normalized * Speed;

        rb.velocity = Velocity;
        transform.LookAt(Target);

        if (fightTime)
            current_state = AIState.EnterCombat;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HArea")
            Hit();

        if (other.gameObject.tag == "FArea")
        {
            fightTime = true;
            current_state = AIState.EnterCombat;
        }

        if (other.gameObject.tag == "Projectile")
        {
            health -= 25;
            Destroy(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Farea")
        {
            current_state = AIState.Chase;
        }
    }

}
