﻿using UnityEngine;
using System.Collections;

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

    private float speed = 0.0f;
    private float direction = 0f;
    private float horizontal = 0.0f;
    private float vertical = 0.0f;
    private float charAngle;
    private AnimatorStateInfo stateInfo;

    private int m_LocomotionId = 0;
    private int m_LocomotionIdPivotLId = 0;
    private int m_LocomotionIdPivotRId = 0;
    private int m_Jump = 0;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator.layerCount >= 2)
        {
            animator.SetLayerWeight(1, 1);
        }

        m_LocomotionId = Animator.StringToHash("Base Layer.Locomotion");
        m_LocomotionIdPivotLId = Animator.StringToHash("Base Layer.LocomotionPivotL");
        m_LocomotionIdPivotRId = Animator.StringToHash("Base Layer.LocomotionPivotR");
        m_Jump = Animator.StringToHash("Base Layer.jump");
    }

    // Update is called once per frame
    void Update() {

        if (animator)
        {

            stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            charAngle = 0.0f;
            direction = 0.0f;

            StickToWorldSpace(this.transform, gamecam.transform, ref direction, ref speed, ref charAngle, IsInPivot());
            animator.SetFloat("Speed", speed);
            animator.SetFloat("Direction", direction, directionDampTime, Time.deltaTime);
               
            if (speed > LocomotionTreshehold)
            {
                if (!IsInPivot())
                {
                    animator.SetFloat("Angle", charAngle);
                }
            }

            if (speed < LocomotionTreshehold && Mathf.Abs(h) < 0.5f)
            {
                animator.SetFloat("Direction", 0f);
                animator.SetFloat("Angle", 0f);
            }

        }
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
        return stateInfo.nameHash == m_LocomotionIdPivotLId || stateInfo.nameHash == m_LocomotionIdPivotRId;
    }
}
