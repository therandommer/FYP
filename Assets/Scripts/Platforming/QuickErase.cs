using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickErase : MonoBehaviour
{
	[SerializeField]
	bool hasDisabled = false;
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
				box.GetComponent<BoxCollider2D>().enabled = true;
			}
			if (gc.GetIsBuilding() && box.GetComponent<BoxCollider2D>().enabled == true) //disabled during building
			{
				box.GetComponent<BoxCollider2D>().enabled = false;
			}
		}
		if(!isEnabledGameplay)
		{
			if (isEnabledGameplay)
			{
				if (gc.GetIsGameplay() && box.GetComponent<BoxCollider2D>().enabled == true) //enables during gameplay
				{
					box.GetComponent<BoxCollider2D>().enabled = false;
				}
				if (gc.GetIsBuilding() && box.GetComponent<BoxCollider2D>().enabled == false) //enabled during building
				{
					box.GetComponent<BoxCollider2D>().enabled = true;
				}
			}
		}
    }
}
