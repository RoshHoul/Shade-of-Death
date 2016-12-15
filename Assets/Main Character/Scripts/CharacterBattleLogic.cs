using UnityEngine;
using System.Collections;

public class CharacterBattleLogic : MonoBehaviour {

    public bool inCombat;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "EnemyMob")
            inCombat = true;

        if (coll.gameObject.tag == "EndGame")
            Debug.Log("Game Over"); 
    }

    void OnTriggerExit (Collider coll)
    {
        if (coll.gameObject.tag == "EnemyMob")
            inCombat = false;
    }
}
