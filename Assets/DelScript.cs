using UnityEngine;
using System.Collections;

public class DelScript : MonoBehaviour {


    void onTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "EnemyMob")
            Debug.Log("Mecha v kokala");

    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
