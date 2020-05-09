using UnityEngine;
using UnityEngine.UI;

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
	readonly float xOffset = 5.0f; //change this offset based on if the character is facing left or right, will need to lerp this
	[SerializeField]
	readonly float yOffset = 4.0f;
	[SerializeField]
	readonly float zOffset = -10.0f;
	[SerializeField]
	float moveScale = 3.0f;

	[SerializeField]
	AudioSource buildMusic = null;
	[SerializeField]
	AudioSource playMusic = null;
	[SerializeField]
	float defaultVolume = 0.4f;
	[SerializeField]
	Slider musicSlider = null;

	void Awake()
	{
		defaultPosition.x = transform.position.x;
		defaultPosition.y = transform.position.y;
		defaultPosition.z = transform.position.z;
		gc = FindObjectOfType<GlobalController>();
		build = FindObjectOfType<BuildSettings>();
		//ensuring music is set up correctly and ensures no deafening of user on start up
		musicSlider.value = defaultVolume;
		playMusic.volume = musicSlider.value;
		playMusic.mute = true;
		playMusic.Stop();
		buildMusic.volume = musicSlider.value;
		buildMusic.mute = false;
		buildMusic.Play();
	}

	void ResetPosition()
	{
		Debug.Log("Camera resetting");
		transform.position = defaultPosition;
	}

	void MoveToPlayer()
	{
		transform.position = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, player.transform.position.z + zOffset);
	}
	void Update()
	{
		if (gc.GetIsGameplay())
		{
			player = null;
		}
		//music switch
		if(gc.GetIsGameplay() && playMusic.mute == true)
		{
			buildMusic.mute = true;
			buildMusic.Stop();
			playMusic.mute = false;
			playMusic.Play();
		}
		if (gc.GetIsBuilding() && buildMusic.mute == true)
		{
			playMusic.mute = true;
			playMusic.Stop();
			buildMusic.mute = false;
			buildMusic.Play();
		}
		if(buildMusic.mute == false && buildMusic.volume != musicSlider.value)
		{
			buildMusic.volume = musicSlider.value;
		}
		if(playMusic.mute == false && playMusic.volume != musicSlider.value)
		{
			playMusic.volume = musicSlider.value;
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
		if (player == null && isGameplay)
		{
			player = FindObjectOfType<Player>();
		}
		if (isGameplay && player != null) //follows the payer location with a slight offset to allow for visibility
		{
			MoveToPlayer();
		}
		if (isBoosting)
		{
			isBoosting = false;
			moveScale /= 2;
		}
		if (!isGameplay) //panning/button presses to change positions
		{
			if (Input.GetAxis("Boost") != 0)
			{
				isBoosting = true;
				moveScale *= 2;
			}
			if (Input.GetAxis("Horizontal") != 0)
			{
				if (Input.GetAxis("Horizontal") < 0)
				{
					transform.position += new Vector3(-1 * moveScale, 0, 0);
				}
				if (Input.GetAxis("Horizontal") > 0)
				{
					transform.position += new Vector3(1 * moveScale, 0, 0);
				}
			}
			if (Input.GetAxis("Vertical") != 0)
			{
				if (Input.GetAxis("Vertical") < 0)
				{
					transform.position += new Vector3(0, -1 * moveScale, 0);
				}
				if (Input.GetAxis("Vertical") > 0)
				{
					transform.position += new Vector3(0, 1 * moveScale, 0);
				}
			}
		}
		if (transform.position.x < 19.3f && !isGameplay)
		{
			transform.position = new Vector3(19.3f, transform.position.y, zOffset);
		}
		if (transform.position.x < 19.3f + xOffset && isGameplay)
		{
			transform.position = new Vector3(19.3f + xOffset, transform.position.y, zOffset);
		}
		if (transform.position.y < 10.5f)
		{
			transform.position = new Vector3(transform.position.x, 10.5f, zOffset);
		}
	}

	public void SetDefaultVolume(int _volume)
	{

	}
}
