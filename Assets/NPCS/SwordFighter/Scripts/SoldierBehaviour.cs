using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierBehaviour : MonoBehaviour {

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

    enum AIState { Chase, EnterCombat, Strike}

    AIState current_state;
    private AIState last_state;
    
	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        animator.SetInteger("Speed", 6);
    }
	
	// Update is called once per frame
	void Update ()
    {
        inCombat = hitCheck.GetComponent<CharacterBattleLogic>().inCombat;
        

    }
}
