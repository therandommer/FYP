using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
	BuildSettings buildSettings;
	GlobalController gc;
	[SerializeField]
	BoxCollider2D box = null;
	Vector3 boxSize = new Vector3(0, 0, 0); //default size to prevent large placement when not necessary
	#region held Object
	[SerializeField]
	SpriteRenderer heldObject = null;
	Color baseColour;
	Color errorColour;
	[SerializeField]
	GameObject parentObject = null;
	[SerializeField]
	List<GameObject> placeableObjects = null;
	[SerializeField]
	int heldID = 0;
	[SerializeField]
	Vector2 pointingLocation;
	[SerializeField]
	Vector2 roundedLocation;
	[SerializeField]
	bool isLocationValid = true;
	[SerializeField]
	bool isHoveringInteractable = false;
	[SerializeField]
	float buildDelay = 0.005f;
	[SerializeField]
	float currentBuildDelay;
	[SerializeField]
	bool isErasing = false; //only used for the erase button function, not middle click erase
	[SerializeField]
	float eraseScale = 1.0f; //used explicitly for the erase function
	#endregion

	void Start()
	{
		gc = FindObjectOfType<GlobalController>();
		boxSize = box.size; 
		buildSettings = FindObjectOfType<BuildSettings>();
		heldObject = gameObject.GetComponent<SpriteRenderer>();
		baseColour = Color.white;
		errorColour = Color.red;
		isLocationValid = true;
		isHoveringInteractable = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		DisablePlacement();
		if (other.gameObject.tag == "Interactable")
		{
			isHoveringInteractable = true;
		}
		else if (other.gameObject.tag != "Interactable")
		{
			isHoveringInteractable = false;
		}
		if (Input.GetMouseButtonDown(1) && isHoveringInteractable)
		{
			other.SendMessage("AltInteraction");
		}
		//Debug.Log("Entered trigger");
		heldObject.color = errorColour;
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		DisablePlacement();
		if (other.gameObject.tag == "Interactable")
		{
			isHoveringInteractable = true;
		}
		else if (other.gameObject.tag != "Interactable")
		{
			isHoveringInteractable = false;
		}
		if (Input.GetMouseButtonDown(1) && isHoveringInteractable)
		{
			other.SendMessage("AltInteraction");
		}
		//Debug.Log("Currently Colliding");
		heldObject.color = errorColour;
		if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(2) && heldID == 19 && !isLocationValid)
		{
			other.gameObject.SendMessage("Erase"); //need to put this function in every object somewhere
			isLocationValid = true;
			heldObject.color = baseColour;
		}
		if (Input.GetMouseButtonDown(2) || Input.GetMouseButton(2) && !isLocationValid)
		{
			if(heldID != 19)
			{
				other.gameObject.SendMessage("Erase"); //need to put this function in every object somewhere
				isLocationValid = true;
				heldObject.color = baseColour;
			}
			else
			{
				Debug.Log("Currently erasing is bound to main click");
			}
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if (this.roundedLocation != new Vector2(other.transform.position.x, other.transform.position.y))
		{
			EnablePlacement();
		}
		isHoveringInteractable = false;
		//Debug.Log("Exited trigger");
		heldObject.color = baseColour;
	}

	void Update()
	{
		if (!gc.GetIsBuilding() || gc.GetIsPaused() && heldID != 16) //sets the cursor to a null object while in gameplay or paused
		{
			SetHeldID(16);
			box.enabled = false;
		}
		if (gc.GetIsBuilding() && !gc.GetIsPaused() && heldID == 16)
		{
			SetHeldID(0);
			box.enabled = true;
		}
		if (heldID == 6) //base colour needed for green water
		{
			baseColour = new Color(0, 255, 0);
		}
		else if (heldID != 6) //every other object has white base colour
		{
			baseColour = Color.white;
		}
		if (heldID != 19) //only certain objects are scaled by 3x
		{
			if (heldID == 13 || heldID == 9 || heldID == 10 || heldID == 18) //player,etc. scaling
			{
				transform.localScale = new Vector3(3, 3, 3);
				box.size = boxSize;
			}
			else if (heldID != 13 && heldID != 9 && heldID != 10 && heldID != 18) 
			{
				transform.localScale = new Vector3(1, 1, 1);
				box.size = boxSize;
			}
		}
		else if (heldID == 19 && transform.localScale != new Vector3(eraseScale, eraseScale, eraseScale)) //different while erasing
		{
			transform.localScale = new Vector3(eraseScale, eraseScale, eraseScale); 
	}
		if (heldID == 9) //certain objects need rotating, eg. Fireball
		{
			transform.rotation = Quaternion.Euler(0, 0, 90);
		}
		else if (heldID != 9) //fireball only one rotated
		{
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}

		if (currentBuildDelay > 0.0f)
		{
			currentBuildDelay -= Time.deltaTime;
		}
		if (currentBuildDelay <= 0.0f && isLocationValid) //prevents most duplicate placements
		{
			EnablePlacement();
		}
		pointingLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//keeps objects to a grid based system
		roundedLocation.x = Mathf.RoundToInt(pointingLocation.x);
		roundedLocation.y = Mathf.RoundToInt(pointingLocation.y);
		heldObject.sprite = placeableObjects[heldID].GetComponentInChildren<SpriteRenderer>().sprite; //displays sprite render to player
		this.transform.position = roundedLocation;
		if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
		{
			if (Input.GetMouseButton(0) && isLocationValid && gc.GetIsBuilding() && !gc.GetIsPaused()) 
			{
				if(heldID != 19)
				{
					Instantiate(placeableObjects[heldID], new Vector3(roundedLocation.x, roundedLocation.y, 0), transform.rotation, parentObject.transform);
					DisablePlacement();
					currentBuildDelay = buildDelay;
					if (heldID == 13 || heldID == 14) //reset to empty block after player is placed, prevents multiple spawns
					{
						SetHeldID(15); //allows player to specify next block
					}
					currentBuildDelay = buildDelay;
				}
				else
				{
					Debug.Log("Doing something else with left click");
				}
			}
		}
	}
	#region setters and getters
	public void SetHeldID(int _id)
	{
		heldID = _id;
	}
	public int GetHeldID()
	{
		return heldID;
	}
	public void SetIsErasing(bool _isErasing)
	{
		isErasing = _isErasing;
	}
	public void SetEraseScale(float _eraseScale)
	{
		eraseScale = _eraseScale;
	}
	public void DisablePlacement()
	{
		Debug.Log("Disabled");
		isLocationValid = false;
	}
	public void EnablePlacement()
	{
		isLocationValid = true;
	}
	public List<GameObject> GetPlaceableObjects()
	{
		return placeableObjects;
	}
	public Color GetErrorColour()
	{
		return errorColour;
	}
	#endregion
}