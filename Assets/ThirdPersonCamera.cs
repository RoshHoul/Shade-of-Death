using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {

    [SerializeField]
    private float distanceAway;
    [SerializeField]
    private float distanceUp;
    [SerializeField]
    private float smooth;
    [SerializeField]
    private Transform follow;
    private Vector3 targetPosition;


    // Use this for initialization
    void Start () {
        follow = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        //setting the target position to be the correct offset from the hovercraft
        targetPosition = follow.position + Vector3.up * distanceUp - Vector3.forward * distanceAway;
        Debug.DrawRay(follow.position, Vector3.up * distanceUp, Color.red);
        Debug.DrawRay(follow.position, -1 * follow.forward * distanceAway, Color.blue);
        Debug.DrawLine(follow.position, targetPosition, Color.magenta);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);

        transform.LookAt(follow);
    }
}
