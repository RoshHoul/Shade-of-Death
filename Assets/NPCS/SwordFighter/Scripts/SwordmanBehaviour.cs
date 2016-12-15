using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwordmanBehaviour : MonoBehaviour {

    [SerializeField]
    public Animator animator;

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
    Rigidbody rb;

    public float curTime = 0f;
    private float pauseDuration = 3f;
    private int m_Speed = 2;
    private float randFloat = 0.0f;
    private int targetF;
    private int sizeList;

    bool inCombat;
    public bool canFight;
    public bool canCharge = false;


    enum AIState { Chase, EnterCombat, Strike }

    AIState current_state;
    private AIState last_state;

    // Use this for initialization
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        animator.SetInteger("Speed", 6);
        current_state = AIState.Chase;
        
    }

    void Update()
    {
        inCombat = hitCheck.GetComponent<CharacterBattleLogic>().inCombat;

        switch (current_state)
        {
            case AIState.Chase:
                ReachTarget();
                break;
            case AIState.EnterCombat:
                EnterFight();
                break;
            case AIState.Strike:
                Hit();
                break;
        }

        last_state = current_state;
      /*  if (canFight == false)
            ReachTarget();
        else if (canFight)
        {
            EnterFight();
            Debug.Log("ne");
        }
        if (canCharge && canFight)
        {
            Debug.Log("Can charge is " + canCharge + " and CanFight is " + canFight);
//            Hit();
        }

        rb.velocity = Velocity; */
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
                Debug.Log("Test 1");
                if (inCombat == false)
                {
                    Debug.Log("Test 2");
                    //                    canCharge = true;
                    //Hit();
                    current_state = AIState.Strike;

                }
            }
        }

        rb.velocity = Velocity;
        transform.LookAt(Enemy.position);
    }
    
    void Hit()
    {
        Target = Enemy.position;
        MoveDirection = Target - transform.position;
        Velocity = rb.velocity;
        Speed = 3;

        Velocity = MoveDirection.normalized * Speed;
        rb.velocity = Velocity;

        if (inCombat)
        {
            transform.LookAt(Target);
//            Debug.Log("Target" + Target);
            MoveDirection = Target.normalized;
//            Debug.Log("MoveDir is " + MoveDirection);
            Velocity = Vector3.zero;
//            Debug.Log("Vec3 is " + Velocity);
            animator.SetBool("canHit", true);
        } 
        Debug.Log("BOI");
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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HArea")
            Hit();

        if (other.gameObject.tag == "FArea")
        {
            current_state = AIState.EnterCombat;
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
