using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGoal : MonoBehaviour
{
	GlobalController gc;

	bool isFinished = false;

    // Start is called before the first frame update
    void Awake()
    {
		gc = FindObjectOfType<GlobalController>().GetComponent<GlobalController>();
    }
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player")
		{
			Debug.Log("Finished the game");
			isFinished = true;
			
		}
		if(isFinished)
		{
			gc.SetIsGameplay(false);
			gc.SetIsBuilding(true);
			collision.SendMessage("Erase"); //replace with victory animation, which erases the player
			//do end goal animation or something here
		}
	}
}
