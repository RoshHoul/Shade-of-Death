﻿// Patrol.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatrolAgent : MonoBehaviour
{
    [SerializeField]
    public Animator animator;

    public Transform Escape;
    public Transform[] Waypoints;
    public float Speed;
    public int curWayPoint;
    public bool doPatrol = true;
    public Vector3 Target;
    public Vector3 MoveDirection;
    public Vector3 Velocity;
    public List<Transform> visibleTargets;
    public int target;
    public bool targetFound = false;

    Rigidbody rb;

    public float curTime = 0f;
    private float pauseDuration = 3f;
    private int m_Speed = 2;
    private float randFloat = 0.0f;
    private int targetF;
    private int sizeList;
    private bool isHit = false;
    System.Random r = new System.Random();

    GameObject[] enemies;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
        //        m_randBool = Animator.StringToHash("Base Layer.randBool");

        animator.SetInteger("Speed", 2);
        visibleTargets = this.GetComponent<FieldOfView>().visibleTargets;
        Debug.Log(Escape.position);

    }

    void Update()
    {

        targetF = this.GetComponent<FieldOfView>().TargetFound;

        //   Debug.Log(targetF);
        if (!isHit)
        {
            if (targetF == 0)
            {
                Patrol();
            }
            else
            {
                EscapeFromReality();
            }
        }
        /*
        if (sizeList > 0)
        {

        }
        */
        rb.velocity = Velocity;
        transform.LookAt(Target);
    }

    void EscapeFromReality()
    {
        Target = Escape.position;
        MoveDirection = Target - transform.position;
        Velocity = rb.velocity;
        animator.SetInteger("Speed", 2);
        Speed = 12;

        Velocity = MoveDirection.normalized * Speed;

        if (MoveDirection.magnitude < 1)
        {
            curTime += Time.deltaTime;
            MoveDirection = Target.normalized;
            Velocity = Vector3.zero;
            Debug.Log("Tam sum");
            animator.SetInteger("Speed", 0);
            /*            randFloat = Random.value;
                        animator.SetFloat("randIdle", randFloat);

                        if (curTime >= pauseDuration)
                        {
                            curWayPoint++;
                            curTime = 0;
                            animator.SetInteger("Speed", 1);

                        } */
        }

        rb.velocity = Velocity;
        transform.LookAt(Target);
    }

    void Patrol()
    {
        if (curWayPoint < Waypoints.Length)
        {
            Target = Waypoints[curWayPoint].position;
            MoveDirection = Target - transform.position;
            Velocity = rb.velocity;
            animator.SetInteger("Speed", 1);

            if (MoveDirection.magnitude < 1)
            {
                curTime += Time.deltaTime;
                MoveDirection = Target.normalized;
                Velocity = Vector3.zero;
                animator.SetInteger("Speed", 0);
                randFloat = Random.value;
                animator.SetFloat("randIdle", randFloat);

                if (curTime >= pauseDuration)
                {
                    curWayPoint++;
                    curTime = 0;
                    animator.SetInteger("Speed", 1);

                }
            }
            else
                Velocity = MoveDirection.normalized * Speed;
        }

        else
        {
            if (doPatrol)
                curWayPoint = 0;
            else
                Velocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Debug.Log("MINAVASH PREZ MEEEN");
            Destroy(other.gameObject);
            animator.SetBool("isHit", true);
            Rigidbody r = GetComponent<Rigidbody>();
            r.isKinematic = true;
        }
    }

    void OnTriggerEnter(Collider alarm)
    {
        if (alarm.gameObject.tag == "Alarm")
        {
            targetFound = true;
            Debug.Log("VIKAI");
           enemies = GameObject.FindGameObjectsWithTag("EnemyMob");
            foreach (GameObject enemy in enemies )
            {
//                enemy.GetComponent<Rigidbody>().isKinematic = false;
                
            }
        }
    }
}