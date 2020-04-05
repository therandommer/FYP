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
	bool isColliding = false;
    [SerializeField]
    bool isHoveringInteractable = false;
	[SerializeField]
	float buildDelay = 0.005f;
	[SerializeField]
	float currentBuildDelay;
	#endregion

	void Start()
    {
		gc = FindObjectOfType<GlobalController>();
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
		isColliding = true;
        if(other.gameObject.tag == "Interactable")
        {
            isHoveringInteractable = true;
        }
        else if(other.gameObject.tag != "Interactable")
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
		isColliding = true;
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
		if (Input.GetMouseButtonDown(2) || Input.GetMouseButton(2) && !isLocationValid)
		{
			other.gameObject.SendMessage("Erase"); //need to put this function in every object somewhere
			isLocationValid = true;
			heldObject.color = baseColour;
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
		if(gc.GetIsBuilding() && !gc.GetIsPaused() && heldID == 16)
		{
			SetHeldID(0);
			box.enabled = true;
		}
		if(heldID == 6) //base colour needed for green water
		{
			baseColour = new Color(0, 255, 0);
		}
		else if(heldID != 6) //every other object has white base colour
		{
			baseColour = Color.white;
		}
		if(heldID == 13) //player scaling
		{
			transform.localScale = new Vector3(3, 3, 3);
		}
		else if(heldID != 13) //only player is scaled by 3x
		{
			transform.localScale = new Vector3(1, 1, 1);
		}
		if(currentBuildDelay>0.0f)
		{
			currentBuildDelay -= Time.deltaTime;
		}
		if(currentBuildDelay <= 0.0f && isLocationValid) //prevents most duplicate placements atm
		{
			EnablePlacement();
		}
		pointingLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//keeps objects to a grid based system
		roundedLocation.x = Mathf.RoundToInt(pointingLocation.x);
		roundedLocation.y = Mathf.RoundToInt(pointingLocation.y);
		heldObject.sprite = placeableObjects[heldID].GetComponent<SpriteRenderer>().sprite; //displays sprite render to player
		this.transform.position = roundedLocation;
        if(!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) 
        {
            if (Input.GetMouseButton(0) && isLocationValid && heldID <= 14) //probably change this limitter later
            {
				Instantiate(placeableObjects[heldID], new Vector3(roundedLocation.x, roundedLocation.y, 0), transform.rotation,parentObject.transform);
				DisablePlacement();
				currentBuildDelay = buildDelay;
				if (heldID == 13 || heldID == 14) //reset to empty block after player is placed, prevents multiple spawns
				{
					SetHeldID(15); //allows player to specify next block
				}
				currentBuildDelay = buildDelay;
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
    public void DisablePlacement()
    {
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
