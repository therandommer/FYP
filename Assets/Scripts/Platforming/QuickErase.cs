﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickErase : MonoBehaviour
{
	[SerializeField]
	bool isEnabledGameplay = true; //for colliders enabled during gameplay
	[SerializeField]
	BoxCollider2D box = null;
	[SerializeField]
	GlobalController gc = null;

    void Start()
    {
		box = GetComponent<BoxCollider2D>();
		gc = FindObjectOfType<GlobalController>();
    }
    void Update()
    {
		if(isEnabledGameplay)
		{
			if (gc.GetIsGameplay() && box.GetComponent<BoxCollider2D>().enabled == false) //enables during gameplay
			{
				Debug.Log("Enabled trigger box");
				box.GetComponent<BoxCollider2D>().enabled = true;
			}
			if (gc.GetIsBuilding() && box.GetComponent<BoxCollider2D>().enabled == true) //disabled during building
			{
				Debug.Log("Disabled trigger box");
				box.GetComponent<BoxCollider2D>().enabled = false;
			}
		}
		if(!isEnabledGameplay)
		{
			if (isEnabledGameplay)
			{
				if (gc.GetIsGameplay() && box.GetComponent<BoxCollider2D>().enabled == true) //enables during gameplay
				{
					Debug.Log("Disabled trigger box");
					box.GetComponent<BoxCollider2D>().enabled = false;
				}
				if (gc.GetIsBuilding() && box.GetComponent<BoxCollider2D>().enabled == false) //enabled during building
				{
					Debug.Log("Enabled trigger box");
					box.GetComponent<BoxCollider2D>().enabled = true;
				}
			}
		}
    }
}
