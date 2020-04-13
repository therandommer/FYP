using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	BaseObject obj = null;
	GlobalController gc = null;
	Rigidbody2D rb = null;
	[SerializeField]
	bool isMoving = false; //triggered while paused/building
	[SerializeField]
	bool hasActivated = false; //will only move while activated
	[SerializeField]
	bool initialFlip = false;
	[SerializeField]
	bool isFacingForward = true; //determines the direction this object travels
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
		if(collision.gameObject.tag == "Camera" && gc.GetIsGameplay() && !hasActivated)
		{
			Debug.Log("1");
			hasActivated = true;
			isMoving = true;
			isFacingForward ^= true; //inverts the flip
			FlipThis();
		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Camera" || gc.GetIsGameplay() && !hasActivated)
		{
			Debug.Log("2");
			hasActivated = true;
			isMoving = true;
		}
	}
	void Update()
    {
		if(gc.GetIsGameplay() && !initialFlip)
		{
			FlipThis();
			initialFlip = true;
		}
		if(isMoving && currentDelay > 0.0f)
		{
			isMoving = false;
		}
		if(currentDelay > 0.0f)
		{
			currentDelay -= Time.deltaTime;
		}
		if(currentDelay <= 0.0f && !gc.GetIsPaused())
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
			Debug.Log("Moving");
			isMoving = true;
		}
		if (gc.GetIsGameplay() && isMoving) //allows projectile to move at a static rate
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
				Debug.Log("Rotating");
				thisVelocity = Vector3.zero;
				FlipThis();
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
    }
	void StartTimer()
	{
		if(currentDelay<=0.0f)
		{
			currentDelay = stopDelay;
			isMoving = false;
			FlipThis();
		}
	}
	void FlipThis()
	{
		Debug.Log("Flip");
		this.transform.Rotate(0, 0, 180.0f, Space.Self);
		if(isFacingForward == true)
		{
			isFacingForward = false;
		}
		else if(isFacingForward == false)
		{
			isFacingForward = true;
		}
	}
	void ResetThis()
	{
		Debug.Log("1");
		currentCharges = maxCharges;
		isMoving = false;
		isFacingForward = true;
		hasActivated = false;
		this.transform.position = obj.GetDefaultPosition();
	}
}
