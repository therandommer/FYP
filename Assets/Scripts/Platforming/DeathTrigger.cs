using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			collision.SendMessage("Defeat");
		}
		if (collision.gameObject.tag == "Hazard")
		{
			collision.SendMessage("StartTimer");
		}
	}
}
