using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	float speed = 2.0f;
	float timeLeft = 3.0f;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag != "Enemy")
		{
			Destroy(this.gameObject);
		}
	}

	void Update()
    {
        if(timeLeft >0.0f)
		{
			transform.position += transform.right * speed  * Time.deltaTime;
			timeLeft -= Time.deltaTime;
		}
		else
		{
			Destroy(this.gameObject);
		}
    }
}
