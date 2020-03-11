using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// add the control scheme to this script, referencing the "Move" function to handle everything needed (theoretically)
/// </summary>
public class Player : MonoBehaviour
{
	#region movement and collision values
	[Header ("Movement and Collision")]

	[Header("Core Values")]
	[SerializeField]
	float jumpForce = 7.0f;
	[SerializeField]
	float moveScalar = 10.0f;
	[SerializeField] [Range(0,1)]
	float crouchSpeed = 0.3f; //scalar for movement while crouching

	[Header("Guard Conditions")]
	[SerializeField]
	float groundRadius = 0.2f;
	[SerializeField]
	bool isGrounded = false; //checks if touching ground
	[SerializeField]
	float ceilingRadius = 0.2f;
	[SerializeField]
	bool isCeiling = false; //checks if touching ceiling. If both true, die.
	[SerializeField]
	bool isFacingRight = true;
	[SerializeField]
	bool isNormalJump = true; //if the player can move while not grounded
	[SerializeField]
	float moveHorizontal = 0;
	float moveVertical = 0;
	[SerializeField]
	Vector2 movementVector = new Vector2();
	[SerializeField]
	Vector3 thisVelocity = new Vector3();
	[SerializeField]
	float maxXVelocity = 6.0f;
	[SerializeField]
	float minXVelocity = -6.0f;
	[SerializeField]
	float maxYVelocity = 6.0f;
	[SerializeField]
	float minYVelocity = -10.0f;

	[Header("Other Values")]
	[SerializeField]
	LayerMask whatIsGround;
	[SerializeField]
	Transform groundCheck; //where player finds floor
	[SerializeField]
	Transform ceilingCheck; //where player finds ceiling
	[SerializeField]
	Collider2D crouchCollider; //determines which collider to disable while crouching
	[SerializeField]
	Rigidbody2D rb;
	#endregion

	BuildSettings build;
	[SerializeField]
	private int maxHealth = 5;
	private int health = 0;
	private bool isPowered = false;
	private bool needsPowerup = false;
	private float maxITime = 1.0f; //time can't be hit
	private float currentITime = 0.0f; //used for timer


	private void Awake()
	{
		health = maxHealth;
		build = FindObjectOfType<BuildSettings>();
		rb = GetComponent<Rigidbody2D>();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag=="Ladder" && this.transform.position.y<collision.transform.position.y + 0.5f)
		{
			isNormalJump = false;
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.tag=="Ladder" && this.transform.position.y>other.transform.position.y + 0.5)
		{
			isNormalJump = true;
			isGrounded = true;
		}
	}
	private void FixedUpdate()
	{
		#region movement and grounding
		//ground check
		isGrounded = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; ++i) //if the groundcheck circle collides with the correct layer, sets grounded to true
		{
			if(colliders[i].gameObject != gameObject)
			{
				Debug.Log("Setting grounded to true");
				isGrounded = true;
			}
		}

		///modifying movement vector based on input
		//horizontal movement
		float moveHorizontal = Input.GetAxis("Horizontal") * moveScalar;
		//vertical movement
		if (Input.GetAxis("Jump") != 0 || Input.GetAxis("Vertical") !=0)
		{
			//regular jumps
			if(isNormalJump && isGrounded)
			{
				moveVertical = Input.GetAxis("Jump") * jumpForce;
				isGrounded = false;
			}
			if(isNormalJump && Input.GetAxis("Vertical") > 0 && isGrounded)
			{
				moveVertical = Input.GetAxis("Vertical") * jumpForce;
				isGrounded = false;
			}
			/*if (!isNormalJump && Input.GetAxis("Vertical") != 0) //only for ladders/vines, etc.
			{
				Vector3 tmpVelocity = new Vector3(0, rb.velocity.y, 0);
				moveHorizontal = 0;
				if (Input.GetAxis("Vertical") < 0)
				{
					moveVertical = Input.GetAxis("Vertical") * -(jumpForce / 2);
				}
				else if (Input.GetAxis("Vertical") > 0)
				{
					moveVertical = Input.GetAxis("Vertical") * (jumpForce / 2);
				}
			}*/
		}
		movementVector = new Vector2(moveHorizontal, moveVertical);
		//flipping sprite to look like it's moving the correct direction
		if(movementVector.x>0 && !isFacingRight)
		{
			Flip();
		}
		else if(movementVector.x<0 && isFacingRight)
		{
			Flip();
		}
		if(movementVector.x>10.0f)
		{
			movementVector.x = 10.0f;
		}
		rb.AddForce(movementVector);
		moveVertical = 0.0f;
		thisVelocity = rb.velocity;
		//clamping velocities to limit max speeds
		if(thisVelocity.x > maxXVelocity)
		{
			thisVelocity.x = maxXVelocity;
		}
		if (thisVelocity.x < minXVelocity)
		{
			thisVelocity.x = minXVelocity;
		}
		if(thisVelocity.y > maxYVelocity)
		{
			thisVelocity.y = maxYVelocity;
		}
		if(thisVelocity.y < minYVelocity)
		{
			thisVelocity.y = minYVelocity;
		}
		rb.velocity = thisVelocity;
		#endregion
	}
	private void Update()
	{
		if(currentITime > 0.0f)
		{
			currentITime -= Time.deltaTime;
		}
	}
	public void Damaged(int _amount)
	{
		if(currentITime <= 0.0f)
		{
			if (!isPowered)
			{
				health -= _amount;
				if(health <= 0)
				{
					Defeat();
				}
				currentITime = maxITime;
			}
			if (isPowered)
			{
				isPowered = false;
				currentITime = maxITime;
			}
		}
	}
	public void Defeat()
	{
		//some animation maybe?
		health = maxHealth;
		this.transform.position = build.GetPlayerSpawn();
	}
	private void Flip()
	{
		//switches isFacingRight to other value
		isFacingRight = !isFacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1; //inverts x scale
		transform.localScale = theScale;
	}
}
