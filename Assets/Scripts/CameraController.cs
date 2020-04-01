using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	Vector3 defaultPosition = new Vector3();
	[SerializeField]
	Player player;
	[SerializeField]
	GlobalController gc;
	[SerializeField]
	BuildSettings build;
	[SerializeField]
	bool isGameplay = false;
	[SerializeField]
	bool isBoosting = false;
	[SerializeField]
	float xOffset = 5.0f; //change this offset based on if the character is facing left or right, will need to lerp this
	[SerializeField]
	float yOffset = 4.0f;
	[SerializeField]
	float zOffset = -10.0f;
	[SerializeField]
	float moveScale = 3.0f;

	void Awake()
	{
		defaultPosition.x = this.transform.position.x;
		defaultPosition.y = this.transform.position.y;
		defaultPosition.z = this.transform.position.z;
		gc = FindObjectOfType<GlobalController>();
		build = FindObjectOfType<BuildSettings>();
	}

	void ResetPosition()
	{
		Debug.Log("Camera resetting");
		this.transform.position = defaultPosition;
	}

	void MoveToPlayer()
	{
		this.transform.position = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, player.transform.position.z + zOffset);
	}
	void Update()
	{
		if (gc.GetIsGameplay())
		{
			player = FindObjectOfType<Player>();
		}
		//switching states of camera
		if (gc.GetIsGameplay() && isGameplay == false)
		{
			isGameplay = true;
		}
		if (!gc.GetIsGameplay() && isGameplay == true)
		{
			isGameplay = false;
			ResetPosition();
		}
		if (isGameplay) //follows the payer location with a slight offset to allow for visibility
		{
			MoveToPlayer();
		}
		if(isBoosting)
		{
			isBoosting = false;
			moveScale /= 2;
		}
		if (!isGameplay) //panning/button presses to change positions
		{
			if(Input.GetAxis("Boost") != 0)
			{
				isBoosting = true;
				moveScale *= 2;
			}
			if(Input.GetAxis("Horizontal") != 0)
			{
				if(Input.GetAxis("Horizontal") < 0)
				{
					this.transform.position += new Vector3(-1 * moveScale, 0, 0);
				}
				if(Input.GetAxis("Horizontal") > 0)
				{
					this.transform.position += new Vector3(1 * moveScale, 0, 0);
				}
			}
			if(Input.GetAxis("Vertical") != 0)
			{
				if(Input.GetAxis("Vertical") < 0)
				{
					this.transform.position += new Vector3(0, -1 * moveScale, 0);
				}
				if(Input.GetAxis("Vertical") > 0)
				{
					this.transform.position += new Vector3(0, 1 * moveScale, 0);
				}
			}
		}
		if(this.transform.position.x < 19.3f && !isGameplay)
		{
			transform.position = new Vector3(19.3f, transform.position.y, zOffset);
		}
		if(this.transform.position.x < 19.3f + xOffset && isGameplay)
		{
			transform.position = new Vector3(19.3f + xOffset, transform.position.y, zOffset);
		}
		if(this.transform.position.y < 10.5f)
		{
			transform.position = new Vector3(transform.position.x, 10.5f, zOffset);
		}
	}
}
