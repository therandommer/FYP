using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
		{
			collision.SendMessage("Defeat");
		}
		if (collision.gameObject.tag == "Hazard")
		{
			collision.SendMessage("StartTimer");
		}
	}
}
