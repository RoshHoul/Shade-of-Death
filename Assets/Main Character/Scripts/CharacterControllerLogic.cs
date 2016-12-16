using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterControllerLogic : MonoBehaviour {

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float directionDampTime = .25f;
    [SerializeField]
    private ThirdPersonCamera gamecam;
    [SerializeField]
    private float directionSpeed = 1.5f;
    [SerializeField]
    private float rotationDegreePerSecond = 120f;
    [SerializeField]
    private float speedDampTime = 0.05f;

    public int health = 100;
    private float speed = 0.0f;
    private float direction = 0f;
    private float horizontal = 0.0f;
    private float vertical = 0.0f;
    private float charAngle;
    private AnimatorStateInfo stateInfo;
    private AnimatorTransitionInfo transInfo;


    private int m_Idle = 0;
    private int m_LocomotionId = 0;
    private int m_LocomotionPivotLId = 0;
    private int m_LocomotionPivotRId = 0;
    private int m_LocomotionPivotLTransId = 0;
    private int m_LocomotionPivotRTransId = 0;
    private bool nextAttack;
    private int m_Jump = 0;

    public Text gameOverText;
    public GameObject sword;
    public GameObject projectile;
    GameObject firePos;
    public float LocomotionTreshehold { get { return 0.2f; } }
    public bool hitFinished = false;
    private bool amIHit;

    AnimationEvent ae = new AnimationEvent();
/*
    private IEnumerator Wait(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
 //            yield return new WaitUntil()

            animator.SetBool("TakeHit", false);
        }
    } */

    // Use this for initialization
    void Start()
    {
        firePos = GameObject.FindGameObjectWithTag("FirePos");
        animator = GetComponent<Animator>();

        if (animator.layerCount >= 2)
        {
            animator.SetLayerWeight(1, 1);
        }

        m_LocomotionId = Animator.StringToHash("Base Layer.Locomotion");
        m_LocomotionPivotLId = Animator.StringToHash("Base Layer.LocomotionPivotL");
        m_LocomotionPivotRId = Animator.StringToHash("Base Layer.LocomotionPivotR");
        m_LocomotionPivotLTransId = Animator.StringToHash("Base Layer.Locomotion -> Base Layer.LocomotionPivotL");
        m_LocomotionPivotRTransId = Animator.StringToHash("Base Layer.Locomotion -> Base Layer.LocomotionPivotR");
        m_Idle = Animator.StringToHash("Base Layer.IdleNonCombat");
        m_Jump = Animator.StringToHash("Base Layer.jump");


        AnimationEvent ae = new AnimationEvent();
        ae.messageOptions = SendMessageOptions.DontRequireReceiver;


    }

    // Update is called once per frame
    void Update() {
        if (animator)
        {
            

            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            transInfo = animator.GetAnimatorTransitionInfo(0);

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            charAngle = 0.0f;
            direction = 0.0f;

            StickToWorldSpace(this.transform, gamecam.transform, ref direction, ref speed, ref charAngle, IsInPivot());
            animator.SetFloat("Speed", speed, speedDampTime, Time.deltaTime);
            animator.SetFloat("Direction", direction, directionDampTime, Time.deltaTime);

            if (speed > LocomotionTreshehold)
            {
                if (!IsInPivot())
                {
                    animator.SetFloat("Angle", charAngle);
                }
            }

            if (speed < LocomotionTreshehold && Mathf.Abs(horizontal) < 0.5f)
            {
                animator.SetFloat("Direction", 0f);
                animator.SetFloat("Angle", 0f);
            }
            //            Debug.Log("Is it in pivot?" + IsInPivot());

            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("Hit", true);
                GameObject ball = Instantiate(projectile) as GameObject;
                ball.transform.position = firePos.transform.position;
                Rigidbody r = ball.GetComponent<Rigidbody>();
                //               if ((animator.IsInTransition(0) && (animator.GetNextAnimatorStateInfo(0).nameHash == m_Idle)))
                //               {
                Debug.Log("strelqm");
                r.AddForce(firePos.transform.forward * 50, ForceMode.Impulse);
                //                r.velocity = new Vector3(0,0,100);
                //               }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                animator.SetBool("Hit", false);
            }

            //if (sword.GetComponent<SwordCheck>().hitFinished)
            //{
            //    animator.SetBool("TakeHit", true);
            //    //                StartCoroutine(Wait(0.1f));
            //}

            //if (animator.GetCurrentAnimatorStateInfo(0).IsName("AnimationName"))
            //{
            //    animator.SetBool("TakeHit", false);
            //    amIHit = false;
            //}
/*            if (!sword.GetComponent<SwordCheck>().readyToMove) 
            {
                animator.SetBool("TakeHit", false);
            }
            */

//            Debug.Log(health);
            
            if (IsDeath()) {
                animator.SetInteger("Health", 0);
                gameOverText.enabled = true;
            }


            
        }
    }

    void getHit()
    {
        animator.SetBool("Take hit", true);
    }

    void getOkey()
    {
        animator.SetBool("Take hit", false);
    }

    bool IsDeath()
    {
        if (health <= 0) 
            return true;
        else
            return false;
    }

    void FixedUpdate()
    {

        if (IsInLocomotion() && ((direction >= 0 && horizontal >= 0) || (direction < 0 && horizontal < 0))) {
            Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, rotationDegreePerSecond * (horizontal < 0f ? -1f : 1f), 0f), Mathf.Abs(horizontal));
            Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
            this.transform.rotation = (this.transform.rotation * deltaRotation);

            if (Input.anyKeyDown)
            {
                animator.SetBool("Jump", true);
            }
        }
    }

    public void StickToWorldSpace(Transform root, Transform camera, ref float directionOut, ref float speedOut, ref float angleOut, bool isPivoting)
    {
        Vector3 rootDirection = root.forward;

        Vector3 stickDirection = new Vector3(horizontal, 0, vertical);

        speedOut = stickDirection.sqrMagnitude;

        Vector3 CameraDirection = camera.forward;
        CameraDirection.y = 0.0f;
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, CameraDirection);

        Vector3 moveDirection = referentialShift * stickDirection;
        Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

        //      Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);

        float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);

        if (!isPivoting)
        {
            angleOut = angleRootToMove;
        }

        angleRootToMove /= 180f;

        directionOut = angleRootToMove * directionSpeed;
    }

    public bool IsInLocomotion()
    {
        return stateInfo.nameHash == m_LocomotionId;
    }

    public bool IsInPivot()
    {
        return stateInfo.nameHash == m_LocomotionPivotLId || 
            stateInfo.nameHash == m_LocomotionPivotRId ||
            transInfo.nameHash == m_LocomotionPivotLTransId || 
            transInfo.nameHash == m_LocomotionPivotRTransId;
    }


    void onTriggerExit(Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
            hitFinished = false;

    }
}
