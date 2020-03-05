using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
	Vector2 thisPosition;
	BuildSettings build;
	GlobalController gc;
	[SerializeField]
	Player player;
    void Awake()
    {
		thisPosition.x = this.transform.position.x;
		thisPosition.y = this.transform.position.y;
		build = FindObjectOfType<BuildSettings>();
		gc = FindObjectOfType<GlobalController>();
		build.SetPlayerSpawn(thisPosition);
    }

    void Update()
    {
		thisPosition.x = this.transform.position.x;
		thisPosition.y = this.transform.position.y;
		if (thisPosition!=build.GetPlayerSpawn())
		{
			build.SetPlayerSpawn(thisPosition);
		}
		//hide any collisions/graphics
		if(!gc.GetIsBuilding())
		{
			
		}
		else if(gc.GetIsBuilding())
		{
			
		}

    }
}
