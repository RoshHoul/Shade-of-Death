using UnityEngine;
using System.Collections;

public class MainCharacter : MonoBehaviour {

    private Animator animComp;
    private int movementState;
    private int prevMovementState;
    private enum CharacterMovement {IDLE = 0, WALK = 1, RUN = 2, JUMP = 3, PUSH = 4 };

    public Transform mainCamera;

    void Start()
    {
        animComp = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
             prevMovementState = movementState;
            movementState = (int)CharacterMovement.JUMP;
        }
        else if (movementState == (int) CharacterMovement.JUMP)
        {
            if (GetAnimatorState() != (int)CharacterMovement.JUMP)
            {
                movementState = prevMovementState;
            }
        }

        if (Input.GetKeyDown(KeyCode.W)) 
        {
            movementState = (int)CharacterMovement.RUN;
        } else if( Input.GetKeyUp(KeyCode.W))
        {
            movementState = (int)CharacterMovement.IDLE;
        }

        animComp.SetInteger("PlayerState", movementState);

        this.transform.rotation = mainCamera.rotation;

    }

    int GetAnimatorState()
    {
        int jumpState = Animator.StringToHash("Base.jump");
        int idleState = Animator.StringToHash("Base.idle");
        int walkState = Animator.StringToHash("Base.walk");
        int runState = Animator.StringToHash("Base.runtest");

        AnimatorStateInfo currentBaseState = animComp.GetCurrentAnimatorStateInfo(0);

        if (currentBaseState.fullPathHash == jumpState)
        {
            return (int)CharacterMovement.JUMP;
        } else if (currentBaseState.fullPathHash == walkState)
        {
            return (int)CharacterMovement.WALK;
        } 
        else if (currentBaseState.fullPathHash == idleState)
        {
            return (int)CharacterMovement.IDLE;
        }
        else if (currentBaseState.fullPathHash == runState)
        {
            return (int)CharacterMovement.RUN;
        }

        return 0;
    }

}

/* HC cont

        public Transform mainCamera;

    private bool isRunning;
    private int movementState;
    private enum CharacterMovement { IDLE = 0, WALK = 1, RUN = 2, JUMP = 3};
	
    // Use this for initialization
	void Start () {
        animComp = this.GetComponent<Animator>();
        animComp.SetInteger("PlayerState", 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            movementState = (int) CharacterMovement.WALK;
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            movementState = (int)CharacterMovement.IDLE;
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }

        if (isRunning == true && movementState == (int)CharacterMovement.WALK)
            movementState = (int)CharacterMovement.RUN;
        else if (isRunning == false && movementState == (int)CharacterMovement.RUN)
            movementState = (int)CharacterMovement.WALK;

        animComp.SetInteger("PlayerState", movementState);

        

    }
    */
