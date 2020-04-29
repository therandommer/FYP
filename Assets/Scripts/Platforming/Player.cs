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
	float jumpForce = 8.0f; ///add defaults to be used while checking if swimming
	[SerializeField]
	float moveScalar = 10.0f;

	[Header("Guard Conditions")]
	[SerializeField]
	float groundRadius = 0.2f;
	[SerializeField]
	bool isGrounded = false; //checks if touching ground
	[SerializeField]
	bool isFacingRight = true;
	[SerializeField]
	bool isNormalJump = true; //if the player can move while not grounded
	[SerializeField]
	float moveHorizontal = 0; //actually used in code later on
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
	float maxYVelocity = 7.0f;
	[SerializeField]
	float minYVelocity = -10.0f;

	[Header("Other Values")]
	[SerializeField]
	LayerMask whatIsGround;
	[SerializeField]
	Transform groundCheck = null; //where player finds floor
	[SerializeField]
	Rigidbody2D rb = null;
	#endregion

	Animator anim = null;
	GlobalController gc = null;
	BuildSettings build = null;
	[SerializeField]
	private int maxHealth = 5;
	private int health = 0;
    private bool isSwimming = false;
	private bool isPowered = false;
	//private bool needsPowerup = false;
    private bool isDefeated = false;
	private float maxITime = 1.0f; //time can't be hit
	private float currentITime = 0.0f; //used for timer

	private void Awake()
	{
		health = maxHealth;
		anim = GetComponent<Animator>();
		gc = FindObjectOfType<GlobalController>();
		build = FindObjectOfType<BuildSettings>();
		rb = GetComponent<Rigidbody2D>();
		moveHorizontal = moveVertical; //gets compiler to be quiet about these values
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Ladder" && this.transform.position.y<collision.transform.position.y + 0.5f)
		{
			isNormalJump = false;
		}
		if(collision.tag == "Hazard") //placeholder, could be replaced by a hazard base class
		{
			Damaged(1);
		}
	}
    private void OnTriggerExit2D(Collider2D other)
	{
		if(other.tag == "Ladder" && this.transform.position.y>other.transform.position.y + 0.5)
		{
			isNormalJump = true;
			isGrounded = true;
			anim.SetBool("isGrounded", true);
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
			if (colliders[i].gameObject != gameObject)
			{
				isGrounded = true;
				anim.SetBool("isGrounded", true);
			}
			else
			{
				anim.SetBool("isGrounded", false); //help prevent animation short loops
			}
		}
		///modifying movement vector based on input
		if (!gc.GetIsPaused())
		{
			//horizontal movement
			float moveHorizontal = Input.GetAxis("Horizontal") * moveScalar;
			//vertical movement
			if (Input.GetAxis("Jump") != 0 || Input.GetAxis("Vertical") != 0)
			{
				//regular jumps
				if (isNormalJump && isGrounded)
				{
					moveVertical = Input.GetAxis("Jump") * jumpForce;
					isGrounded = false;
					anim.SetBool("isGrounded", false);
				}
				if (isNormalJump && Input.GetAxis("Vertical") > 0 && isGrounded)
				{
					moveVertical = Input.GetAxis("Vertical") * jumpForce;
					isGrounded = false;
					anim.SetBool("isGrounded", false);
				}
			}
			movementVector = new Vector2(moveHorizontal, moveVertical);
			//flipping sprite to look like it's moving the correct direction
			if (movementVector.x > 0 && !isFacingRight)
			{
				Flip();
			}
			else if (movementVector.x < 0 && isFacingRight)
			{
				Flip();
			}
			if (movementVector.x > 10.0f)
			{
				movementVector.x = 10.0f;
			}
			if(movementVector.x != 0)
			{
				anim.SetBool("isWalking", true);
			}
			else
			{
				anim.SetBool("isWalking", false);
			}
			rb.AddForce(movementVector);
			moveVertical = 0.0f;
			thisVelocity = rb.velocity;
			//clamping velocities to limit max speeds
			if (thisVelocity.x > maxXVelocity)
			{
				thisVelocity.x = maxXVelocity;
			}
			if (thisVelocity.x < minXVelocity)
			{
				thisVelocity.x = minXVelocity;
			}
			if (thisVelocity.y > maxYVelocity)
			{
				thisVelocity.y = maxYVelocity;
			}
			if (thisVelocity.y < minYVelocity)
			{
				thisVelocity.y = minYVelocity;
			}
			rb.velocity = thisVelocity;
			#endregion
		}
	}
	private void Update()
	{
		if(currentITime > 0.0f)
		{
			currentITime -= Time.deltaTime;
		}
        if(isSwimming)
        {

        }
		if(!gc.GetIsGameplay())
		{
			Debug.Log("Gameplay ended");
			Erase();
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
	public void Heal (int _amount)
	{
		if(health < maxHealth)
		{
			health += _amount;
			if(health > maxHealth)
			{
				health = maxHealth;
			}
		}
	}
	public void Defeat()
	{
		health = maxHealth;
        isDefeated = true;
		Respawn();
	}
	void Erase()
	{
		Destroy(this.gameObject);
	}
	void Respawn()
	{
		rb.velocity = Vector3.zero;
		this.transform.position = build.GetPlayerSpawn(); //change this to moving to activated checkpoint position
		isDefeated = false;
	}
	public void ResetPosition()
	{
		rb.velocity = Vector3.zero;
		this.transform.position = build.GetPlayerSpawn();
		isDefeated = false;
	}
	private void Flip()
	{
		//switches isFacingRight to other value
		isFacingRight = !isFacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1; //inverts x scale
		transform.localScale = theScale;
	}

	#region setters and getters
	public bool GetIsDefeated()
    {
        return isDefeated;
    }
    public void SetIsDefeated(bool _state)
    {
        isDefeated = _state;
    }
	public int GetHealth()
	{
		return health;
	}
	public int GetMaxHealth()
	{
		return maxHealth;
	}
	#endregion
}
