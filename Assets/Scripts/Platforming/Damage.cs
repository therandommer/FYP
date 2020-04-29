using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
	[SerializeField]
	private int damage = 1;
	[SerializeField]
	private float frequency = 0.5f;
	private float currentDelay = 0.0f;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Player" && currentDelay <= 0.0f)
		{
			collision.gameObject.SendMessage("Damaged", damage);
			currentDelay = frequency;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Player" && currentDelay <= 0.0f)
		{
			collision.gameObject.SendMessage("Damaged", damage);
			currentDelay = frequency;
		}
	}

	void Update()
    {
        if(currentDelay > 0.0f)
		{
			currentDelay -= Time.deltaTime;
		}
    }
}
