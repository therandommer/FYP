using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Basic movement moving in each direction for a few seconds before rotating
public class AIMovement : MonoBehaviour
{
	[SerializeField]
	private float speed = 1.0f;
	[SerializeField]
	private float currentTimer = 0.0f;
	[SerializeField]
	private float maxTimer = 2.0f;
	[SerializeField]
	private bool isFacingRight = false;
	[SerializeField]
	private bool isDefeated = false;
	private GlobalController gc;
	[SerializeField]
	private BaseObject thisObject = null; //link to self in prefab
	[SerializeField]
	private Rigidbody2D rb = null;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		gc = FindObjectOfType<GlobalController>();
	}
	private void Start()
	{
		rb.simulated = false;
		this.transform.localScale = new Vector3(3, 3, 3); //sets to correct scale once awakened
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			SendMessage("Damaged", 1);
		}
	}
	void Update()
    {
		if(gc.GetIsGameplay() && !rb.simulated && !isDefeated)
		{
			Debug.Log("Waking rigidbody");
			rb.velocity = new Vector2(0, 0);
			currentTimer = 0.0f;
			rb.simulated = true;
		}
		else if(gc.GetIsBuilding() && rb.simulated || gc.GetIsPaused() && rb.simulated || isDefeated && gc.GetIsBuilding())
		{
			Debug.Log("Sleeping rigidbody");
			rb.simulated = false;
			rb.velocity = new Vector2(0, 0);
			isDefeated = false;
			currentTimer = 0.0f;
			this.transform.position = thisObject.GetDefaultPosition();
		}
		if(gc.GetIsGameplay() && !gc.GetIsPaused() && !isDefeated)
		{
			if(currentTimer <= 0.0f)
			{
				currentTimer = maxTimer;
			}
			if(currentTimer > 0.0f)
			{
				currentTimer -= Time.deltaTime;
				if(currentTimer <= 0.0f)
				{
					Debug.Log("Flipping");
					Flip();
				}
			}
			transform.Translate(Vector2.right * speed * Time.deltaTime);
		}
    }

	void Flip()
	{
		if (isFacingRight)
		{
			transform.eulerAngles = new Vector3(0, -180, 0);
			isFacingRight = false;
		}
		else
		{
			transform.eulerAngles = new Vector3(0, 0, 0);
			isFacingRight = true;
		}
	}

	public void Defeat()
	{
		Debug.Log("Enemy fell off map");
		isDefeated = true;
		rb.simulated = false;
	}
}
