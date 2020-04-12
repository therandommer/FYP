using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	BaseObject obj = null;
	GlobalController gc = null;
	Rigidbody2D rb = null;
	[SerializeField]
	bool isGameplay = false; //triggered to allow for resets when building
	[SerializeField]
	bool isMoving = false; //triggered while paused/building
	[SerializeField]
	bool hasActivated = false; //will only move while activated
	[SerializeField]
	bool isFacingForward = true; //determines the direction this object travels
	[SerializeField]
	bool hasFlipped = false;
	[SerializeField]
	bool isVertical = true;
	[SerializeField]
	bool hasReachedMax = true; //inverts on camera minimum/original placement
	[SerializeField]
	Vector3 thisVelocity = new Vector3();
	[SerializeField]
	float speed = 4.0f; //speed of projectile going back and forth
	[SerializeField]
	float stopDelay = 2.5f; //used to wait at the bottom
	[SerializeField]
	float currentDelay = 0.0f; //used for timer
	[SerializeField]
	int maxCharges = 10; //number of times object will go up and down 
	[SerializeField]
	int currentCharges = 0;
	int tmp = 0; //used to manipulate velocity

	void Start()
    {
		gc = FindObjectOfType<GlobalController>();
		obj = GetComponent<BaseObject>();
		rb = GetComponent<Rigidbody2D>();
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Camera" && isGameplay && !hasActivated)
		{
			Debug.Log("1");
			hasActivated = true;
			isMoving = true;
			isFacingForward ^= true; //inverts the flip
			hasFlipped = false;
			FlipThis();
		}
		if(collision.gameObject.tag == "Camera" && hasActivated && isMoving && currentDelay <= 0.0f) 
		{
			currentDelay = stopDelay;
			currentCharges--;
			isFacingForward ^= true; //inverts the flip
			hasFlipped = false;
			FlipThis();
		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Camera" && isGameplay && !hasActivated)
		{
			Debug.Log("2");
			hasActivated = true;
			isMoving = true;
		}
	}
	void Update()
    {
		if(isMoving && currentDelay > 0.0f)
		{
			isMoving = false;
		}
		if(currentDelay > 0.0f)
		{
			currentDelay -= Time.deltaTime;
		}
		if(currentDelay <= 0.0f)
		{
			isMoving = true;
		}
		if (gc.GetIsBuilding())
		{
			ResetThis();
		}
		if(gc.GetIsPaused() && hasActivated)
		{
			isMoving = false;
		}
		if(!gc.GetIsPaused() && hasActivated)
		{
			isMoving = true;
		}
		if (isGameplay && isMoving) //allows projectile to move at a static rate
		{
			thisVelocity = rb.velocity;
			if (isFacingForward)
			{
				tmp = 1;
			}
			else if (!isFacingForward)
			{
				tmp = -1;
			}
			if (this.transform.position == obj.GetDefaultPosition() && thisVelocity != Vector3.zero) //prevents spazzing while stopped
			{
				thisVelocity = Vector3.zero;
				isFacingForward ^= true; //rotates object
			}
			else
			{
				if (isVertical)
				{
					thisVelocity.y = speed * tmp;
				}
				else if (!isVertical)
				{
					thisVelocity.x = speed * tmp;
				}
			}
			rb.velocity = thisVelocity;
		}
		if (gc.GetIsGameplay() && !isGameplay) //setting the tag for gameplay updates and build resets later.
		{
			isGameplay = true;
		}
    }
	void FlipThis()
	{
		this.transform.Rotate(0, 0, 180.0f, Space.Self);
		hasFlipped = true;
	}
	void ResetThis()
	{
		Debug.Log("1");
		currentCharges = maxCharges;
		isGameplay = false;
		isMoving = false;
		isFacingForward = true;
		hasFlipped = false;
		hasActivated = false;
		this.transform.position = obj.GetDefaultPosition();
	}
}
