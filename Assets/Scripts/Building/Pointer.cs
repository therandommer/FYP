using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
	BuildSettings buildSettings;
	GlobalController gc;
	[SerializeField]
	BoxCollider2D box;
	#region held Object
	[SerializeField]
	SpriteRenderer heldObject;
	Color baseColour;
	Color errorColour;
	[SerializeField]
	GameObject parentObject;
	[SerializeField]
	List<GameObject> placeableObjects;
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
	float buildDelay = 0.01f;
	[SerializeField]
	float currentBuildDelay;
	#endregion

	void Start()
    {
		gc = FindObjectOfType<GlobalController>();
		buildSettings = FindObjectOfType<BuildSettings>();
		heldObject = gameObject.GetComponent<SpriteRenderer>();
		baseColour = heldObject.color;
		isLocationValid = true;
        isHoveringInteractable = false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		isLocationValid = false;
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
        Debug.Log("Entered trigger");
		heldObject.color = Color.red;
	}
	private void OnTriggerStay2D(Collider2D other)
	{
		isLocationValid = false;
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
        if (Input.GetMouseButtonDown(2) || Input.GetMouseButton(2) && !isLocationValid)
        {
            other.gameObject.SendMessage("Erase"); //need to put this function in every object somewhere
        }
        Debug.Log("Currently Colliding");
		heldObject.color = Color.red;
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		isLocationValid = true;
		isHoveringInteractable = false;
		Debug.Log("Exited trigger");
		heldObject.color = baseColour;
	}

	void Update()
    {
		if (!gc.GetIsBuilding() && heldID != 14)
		{
			SetHeldID(14);
			box.enabled = false;
		}
		if(gc.GetIsBuilding() && heldID == 14)
		{
			SetHeldID(0);
			box.enabled = true;
		}
		if(heldID == 12)
		{
			transform.localScale = new Vector3(3, 3, 3);
		}
		else if(heldID != 12)
		{
			transform.localScale = new Vector3(1, 1, 1);
		}
		if(currentBuildDelay>0.0f)
		{
			currentBuildDelay -= Time.deltaTime;
		}
		pointingLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		roundedLocation.x = Mathf.RoundToInt(pointingLocation.x);
		roundedLocation.y = Mathf.RoundToInt(pointingLocation.y);
		heldObject.sprite = placeableObjects[heldID].GetComponent<SpriteRenderer>().sprite;
		this.transform.position = roundedLocation;
        if(!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(0) && isLocationValid && currentBuildDelay <= 0 && heldID <= 12) //probably change this limitter later
            {
				Instantiate(placeableObjects[heldID], new Vector3(roundedLocation.x, roundedLocation.y, 0), transform.rotation,parentObject.transform);
				if(heldID == 12) //reset to empty block after player is placed, prevents multiple spawns
				{
					SetHeldID(13);
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
    public void DisablePlacement()
    {
        isLocationValid = false;
    }
    public void EnablePlacement()
    {
        isLocationValid = true;
    }
    #endregion
}
