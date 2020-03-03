using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
	BuildSettings build;
	GlobalController gc;
	Vector2 thisPosition;
	private void Awake()
	{
		gc = FindObjectOfType<GlobalController>();
		build = FindObjectOfType<BuildSettings>();
		thisPosition.x = this.transform.position.x;
		thisPosition.y = this.transform.position.y;
		build.SetPlayerSpawn(thisPosition);
	}
	void Start()
	{
	}

    // Update is called once per frame
    void Update()
    {
		if(!gc.GetIsBuilding())
		{
			gameObject.SetActive(false);

		}
		thisPosition.x = this.transform.position.x;
		thisPosition.y = this.transform.position.y;
		if (thisPosition != build.GetPlayerSpawn())
		{
			build.SetPlayerSpawn(thisPosition);
		}
    }
}
