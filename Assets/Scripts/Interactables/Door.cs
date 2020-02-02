using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	[SerializeField]
	int linkNo = 0; //used to change where each door links to. 1-1 2-2, etc.
	Vector2 otherDoor;
	BuildSettings buildSettings;
	void Start()
    {
		buildSettings = FindObjectOfType<BuildSettings>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player" && Input.GetKeyDown(KeyCode.E))
		{
			collision.transform.position = otherDoor;
		}
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Player" && Input.GetKeyDown(KeyCode.E))
		{
			collision.transform.position = otherDoor;
		}
	}

	void Update()
    {
        if(otherDoor == Vector2.zero)
		{
			if(buildSettings.GetDoorList().Count>1)
			{
				for(int i = 0; i<buildSettings.GetDoorList().Count; i++)
				{
					if (buildSettings.GetDoorList()[i].GetComponent<Door>().GetLinkNo() == linkNo && buildSettings.GetDoorList()[i].transform.position != transform.position)
					{
						otherDoor = buildSettings.GetDoorList()[i].transform.position;
					}
				}
			}
		}
    }

	public void SetDoorlinkNo(int _id)
	{
		linkNo = _id;
	}
	public int GetLinkNo()
	{
		return linkNo;
	}
}
