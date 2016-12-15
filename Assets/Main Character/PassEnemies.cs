using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PassEnemies : MonoBehaviour {

    public GameObject director;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "EnemyMob")
        {
            director.GetComponent<AIDirector>().enemies.Add(coll.gameObject);
        }
    }
}
