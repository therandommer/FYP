using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
	BuildSettings buildSettings;
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
	float buildDelay = 0.2f;
	[SerializeField]
	float currentBuildDelay;
	#endregion

	void Start()
    {
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
            if (Input.GetMouseButtonDown(0) && isLocationValid && currentBuildDelay <= 0)
            {
				Instantiate(placeableObjects[heldID], new Vector3(roundedLocation.x, roundedLocation.y, 0), transform.rotation);
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
