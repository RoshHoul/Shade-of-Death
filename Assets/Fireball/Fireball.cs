 using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

    private float timer = 0f;
    private float pauseDuration = 3f;

	// Use this for initialization
	void OnAwake () {
//        Rigidbody r = GetComponent<Rigidbody>();
      
//        r.AddForce(transform.forward * 200, ForceMode.Acceleration);
        
	}
	
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= pauseDuration)
        {
            Destroy(gameObject);
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        
	}
}
