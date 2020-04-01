using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectColour : MonoBehaviour
{
	Pointer point;
	Color baseColour;
	SpriteRenderer sprite;
	// Start is called before the first frame update
	void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
		baseColour = sprite.color;
		point = FindObjectOfType<Pointer>();
	}

	// Update is called once per frame
	void Update()
	{
		if (sprite.color != point.GetComponent<Pointer>().GetErrorColour()) //keeps object with a tint
		{
			sprite.color = baseColour;
		}
	}
}
