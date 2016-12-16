using UnityEngine;
using System.Collections;

public class CharacterBattleLogic : MonoBehaviour {

    public bool inCombat;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(inCombat + " is inCombat");
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "EnemyMob") { 
            inCombat = true;
            Debug.Log("inCombat is: " + inCombat);
        }
    }

    void OnTriggerExit (Collider coll)
    {
        if (coll.gameObject.tag == "EnemyMob")
        {
            inCombat = false;
//            Debug.Log("inCombat is: " + inCombat);
        }
    }
}
