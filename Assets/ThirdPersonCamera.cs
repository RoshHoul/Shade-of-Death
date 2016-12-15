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
    private Transform followXForm;
    [SerializeField]
    private Vector3 offset = new Vector3(0f,1.5f,0f);

    private Vector3 lookDir;
    private Vector3 targetPosition;

    private Vector3 velocityCamSmooth = Vector3.zero;
    [SerializeField]
    private float camSmoothDampTime = 0.1f;



    // Use this for initialization
    void Start () {
        followXForm = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {

    }

    void LateUpdate()
    {
        Vector3 characterOffset = followXForm.position + offset;

        lookDir = characterOffset - this.transform.position;
        lookDir.y = 0;
        lookDir.Normalize();
        Debug.DrawRay(this.transform.position, lookDir, Color.green);

        //setting the target position to be the correct offset from the hovercraft

        targetPosition = characterOffset + followXForm.up * distanceUp - lookDir * distanceAway;

        Debug.DrawRay(followXForm.position, Vector3.up * distanceUp, Color.red);
        Debug.DrawRay(followXForm.position, -1 * followXForm.forward * distanceAway, Color.blue);
        Debug.DrawLine(followXForm.position, targetPosition, Color.magenta);
        CompensateForWalls(characterOffset, ref targetPosition);
        smoothPosition(this.transform.position, targetPosition);

        transform.LookAt(followXForm); 
    }

    public void smoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
    }

    private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
    {
        RaycastHit wallHit = new RaycastHit();

        if(Physics.Linecast(fromObject, toTarget, out wallHit))
        {
            toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
        }
    }
}
