using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBaseObject : MonoBehaviour
{
	int objectID = 0;
	Sprite mouseObject;
	Image thisObject;
	Color startColour;
	Renderer thisRenderer;
	bool isMouseHovering = false;

    void Start()
    {
		startColour = gameObject.GetComponent<Renderer>().material.color;
		thisRenderer = gameObject.GetComponent<Renderer>();
		thisObject = gameObject.GetComponent<Image>();
    }

	private void OnMouseEnter()
	{
		thisRenderer.material.color = Color.gray;
		isMouseHovering = true;
	}
	private void OnMouseExit()
	{
		thisRenderer.material.color = startColour;
		isMouseHovering = false;
	}

	void Update()
    {
		if (Input.GetMouseButtonUp(0) && isMouseHovering)
		{
			//call a function in the pointer
		}
	}
}
