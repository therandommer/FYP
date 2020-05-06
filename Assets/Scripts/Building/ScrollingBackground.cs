using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
	[SerializeField]
	private Transform centerBackground = null;
	[SerializeField]
	private float offset = 30.68f;
    void Update()
    {
		centerBackground.position = new Vector3(centerBackground.position.x, transform.position.y, centerBackground.position.z); //keeps background synced when going up and down.
		if (transform.position.x >= centerBackground.position.x + offset) //transforms once offset is reached.
		{
			centerBackground.position = new Vector3(centerBackground.position.x + offset, transform.position.y, centerBackground.position.z);
		}
		else if(transform.position.x <= centerBackground.position.x - offset)
		{
			centerBackground.position = new Vector3(centerBackground.position.x - offset, transform.position.y, centerBackground.position.z);
		}
		
    }
}
