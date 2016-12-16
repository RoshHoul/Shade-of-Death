using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

    void onTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("YOU WIN");
        }

        if (coll.gameObject.tag == "PlayerBody")
        {
//            hitFinished = true;
            Debug.Log("colission be bace");
        }
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
