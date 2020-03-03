using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
	Vector2 thisPosition;
	BuildSettings build;
	GlobalController gc;
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
        if(gc.GetIsBuilding() && build.GetPlayerSpawned())
		{
			Destroy(GameObject.Find("Player"));
		}
		else if(!gc.GetIsBuilding() && !build.GetPlayerSpawned())
		{
			Instantiate(player);
		}
    }
}
