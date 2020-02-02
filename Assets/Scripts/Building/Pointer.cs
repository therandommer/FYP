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
	#endregion

	void Start()
    {
		buildSettings = FindObjectOfType<BuildSettings>();
		heldObject = gameObject.GetComponent<SpriteRenderer>();
		baseColour = heldObject.color;
		isLocationValid = true;
	}
	public void SetHeldID(int _id)
	{
		heldID = _id;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		isLocationValid = false;
		Debug.Log("Entered trigger");
		heldObject.color = Color.red;
	}
	private void OnTriggerStay2D(Collider2D other)
	{
		isLocationValid = false;
		Debug.Log("Currently Colliding");
		heldObject.color = Color.red;
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		isLocationValid = true;
		Debug.Log("Exited trigger");
		heldObject.color = baseColour;
	}

	void Update()
    {
		pointingLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		roundedLocation.x = Mathf.RoundToInt(pointingLocation.x);
		roundedLocation.y = Mathf.RoundToInt(pointingLocation.y);

		heldObject.sprite = placeableObjects[heldID].GetComponent<SpriteRenderer>().sprite;
		this.transform.position = pointingLocation;
		if (Input.GetMouseButtonDown(0) && isLocationValid)
		{
			Instantiate(placeableObjects[heldID], new Vector3(roundedLocation.x, roundedLocation.y, 0), transform.rotation);
		}
	}
}
