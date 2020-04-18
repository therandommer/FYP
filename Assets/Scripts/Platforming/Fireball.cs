using UnityEngine;

///will go between original location and the bottom/side of the camera
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
	readonly bool isVertical = true;
	[SerializeField]
	bool hasReachedMax = true; //inverts on camera minimum/original placement
	bool didReachMax = false; //used for the states, will be set to false on hitting the bottom
	[SerializeField]
	Vector3 thisVelocity = new Vector3();
	[SerializeField]
	readonly float speed = 4.0f; //speed of projectile going back and forth
	[SerializeField]
	readonly float stopDelay = 2.5f; //used to wait at the bottom
	[SerializeField]
	float currentDelay = 0.0f; //used for timer
	[SerializeField]
	readonly int maxCharges = 10; //number of times object will go up and down 
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
		if (collision.gameObject.tag == "Camera" && gc.GetIsGameplay() && !hasActivated)
		{
			Debug.Log("1");
			hasActivated = true;
			isMoving = true;
		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Camera" || gc.GetIsGameplay() && !hasActivated)
		{
			Debug.Log("2");
			hasActivated = false;
			isMoving = false;
		}
	}
	void Update()
	{
		if (gc.GetIsGameplay() && !initialFlip)
		{
			Debug.Log("Rotating initial");
			FlipThis();
			initialFlip = true;
		}
		if (isMoving && currentDelay > 0.0f)
		{
			isMoving = false;
		}
		if (currentDelay > 0.0f)
		{
			currentDelay -= Time.deltaTime;
		}
		if (currentDelay <= 0.0f && !gc.GetIsPaused())
		{
			isMoving = true;
		}
		if (gc.GetIsBuilding())
		{
			ResetThis();
		}
		if (gc.GetIsPaused() && hasActivated)
		{
			isMoving = false;
		}
		if (!gc.GetIsPaused() && hasActivated)
		{
			if (!isMoving)
			{
				Debug.Log("Moving");
				isMoving = true;
			}
		}
		if (gc.GetIsGameplay() && isMoving) //allows projectile to move at a static rate
		{
			rb.velocity = Vector3.zero;
			thisVelocity = rb.velocity;
			if (isFacingForward)
			{
				tmp = 1;
			}
			else if (!isFacingForward)
			{
				tmp = -1;
			}
			if (transform.position.y >= obj.GetDefaultPosition().y && thisVelocity != Vector3.zero) //prevents spazzing while stopped
			{
				Debug.Log("Offset");
				transform.position -= Vector3.up * 0.15f;
				thisVelocity = Vector3.zero;
				hasReachedMax = true;
				didReachMax = true;
			}
			if (hasReachedMax == true)
			{
				Debug.Log("Reached Max");
				FlipThis();
				hasReachedMax = false;
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
		}
		rb.velocity = thisVelocity;
		if (!isMoving) //stops object when not moving
		{
			Debug.Log("Stopping");
			rb.velocity = Vector3.zero;
		}
	}
	void StartTimer() //called at the bottom of the screen atm. Could also be called from the side of the screen.
	{
		if (currentDelay <= 0.0f && didReachMax) //ensures this only happens once
		{
			currentDelay = stopDelay;
			didReachMax = false;
			//isMoving = false;
			FlipThis();
		}
	}
	void FlipThis()
	{
		if (gc.GetIsGameplay())
		{
			if (isFacingForward == true)
			{
				transform.rotation = Quaternion.Euler(0, 0, 180);
				isFacingForward = false;
			}
			else if (isFacingForward == false)
			{
				transform.rotation = Quaternion.Euler(0, 0, 0);
				isFacingForward = true;
			}
		}
	}
	void ResetThis()
	{
		Debug.Log("1");
		currentCharges = maxCharges;
		isMoving = false;
		isFacingForward = true;
		initialFlip = false;
		transform.rotation = Quaternion.Euler(0, 0, 0);
		hasActivated = false;
		transform.position = obj.GetDefaultPosition();
	}
}
