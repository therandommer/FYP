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
	[SerializeField]
	float moveHorizontal = 0;
	float moveVertical = 0;
	Vector2 movementVector = new Vector2();

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
	GlobalController gc;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		gc = FindObjectOfType<GlobalController>();
	}

	private void FixedUpdate()
	{
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
		//movement
		float moveHorizontal = Input.GetAxis("Horizontal") * moveScalar;
		if (Input.GetAxis("Jump") != 0 && isGrounded)
		{
			moveVertical = Input.GetAxis("Jump") * jumpForce;
		}
		movementVector = new Vector2(moveHorizontal, moveVertical);
		if(movementVector.x>0 && !isFacingRight)
		{
			Flip();
		}
		else if(movementVector.x<0 && isFacingRight)
		{
			Flip();
		}
		rb.AddForce(movementVector);
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
