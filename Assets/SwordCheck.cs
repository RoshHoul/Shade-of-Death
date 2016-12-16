using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwordCheck : MonoBehaviour {

    public Slider healthBar;
    public bool hitFinished = false;
    public bool attackLanded = false;
    public bool readyToMove = false;
    public int counter = 0;
    public GameObject player;


    int blabla;
    private IEnumerator Wait(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {


            hitFinished = true;
            readyToMove = false;
            attackLanded = false;

            blabla = player.GetComponent<CharacterControllerLogic>().health;
            if (counter > 1)
            {
                counter = 0;
            }
            if (counter == 0)
            {
                player.GetComponent<CharacterControllerLogic>().health -= 5;
                healthBar.value -= 0.10f;
                StartCoroutine(Wait(.6f));

                player.GetComponent<Animator>().SetBool("TakeHit", true);
                Debug.Log("CHCL is " + player.GetComponent<CharacterControllerLogic>().health);
                Debug.Log("BLABLA is " + blabla);

                if (player.GetComponent<CharacterControllerLogic>().health != blabla)
                {
                    Debug.Log("1" + readyToMove);
                }
            }
            counter++;

        }
    }

    void OnTriggerExit (Collider coll)
    {
        attackLanded = true;

        player.GetComponent<Animator>().SetBool("TakeHit", false);
//        hitFinished = false;
 //       readyToMove = true;

 //       attackLanded = false;

    }

    void Awake()
    {


    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        //if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("AnimationName"))
        //{
        //    hitFinished = false;
        //}
    }
}
