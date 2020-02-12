using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// add the control scheme to this script, referencing the "Move" function to handle everything needed (theoretically)
/// </summary>
public class Player : MonoBehaviour
{
	[Header("Core Values")]
	[SerializeField]
	float jumpForce = 7.0f;
	[SerializeField]
	float moveScalar = 10.0f;
	[SerializeField] [Range(0,1)]
	float crouchSpeed = 0.3f; //scalar for movement while crouching
	[SerializeField]
	[Range(0, 0.3f)]
	float moveSmooth = 0.05f;
	[SerializeField]
	Vector3 velocity = Vector3.zero;

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
	bool isAirControl = false; //if the player can move while not grounded

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

	[Header("Events")]
	public UnityEvent OnLandEvent;

	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool wasCrouching = false;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		if(OnLandEvent == null)
		{
			OnLandEvent = new UnityEvent();
		}
		if(OnCrouchEvent == null)
		{
			OnCrouchEvent = new BoolEvent();
		}
	}

	private void FixedUpdate()
	{
		bool wasGrounded = isGrounded;
		isGrounded = false;

		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; ++i) //if the groundcheck circle collides with the correct layer, sets grounded to true
		{
			if(colliders[i].gameObject != gameObject)
			{
				isGrounded = true;
				if(!wasGrounded)
				{
					OnLandEvent.Invoke();
				}
			}
		}
	}

	//called to manipulate the player as needed
	//move > 0 = move right, move < 0 = move left
	//crouch = true, enables crouching
	//jump = true starts a jump
	public void Move(float _move, bool _crouch, bool _jump)
	{
		//check if player can stand
		if(!_crouch)
		{
			//if character colliding with ceiling, keep crouching.
			if(Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
			{
				_crouch = true;
			}
		}

		//only allows control of the player if grounded or if they can be controlled in the air
		if(isGrounded || isAirControl)
		{
			if (_crouch)
			{
				if (!wasCrouching)
				{
					wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}
				//Reduce movement speed by crouchSpeed scalar
				_move *= crouchSpeed;

				//when crouching, disable larger collider
				if (crouchCollider != null)
				{
					crouchCollider.enabled = false;
				}
			}
			else
			{
				//reenables larger collider when standing
				if (crouchCollider != null)
				{
					crouchCollider.enabled = true;
				}
				if (wasCrouching)
				{
					wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

				//move character by target velocity
				Vector3 targetVelocity = new Vector2(_move * moveScalar, rb.velocity.y);
				//applies smoothing
				rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, moveSmooth);

				//flipping the player
				if(_move > 0 && !isFacingRight)
				{
					Flip();
				}
				else if (_move < 0 && isFacingRight)
				{
					Flip();
				}
			}
		//handles jumping
		if(isGrounded && _jump)
		{
			//applies vertical force
			isGrounded = false;
			rb.AddForce(new Vector2(0.0f, jumpForce));
		}

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
