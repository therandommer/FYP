using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
	Vector2 thisPosition;
	BuildSettings build;
	[SerializeField]
	SpriteRenderer sprite;
	[SerializeField]
	BoxCollider2D col;
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
			sprite.enabled = false;
			col.enabled = false;
		}
		else if(gc.GetIsBuilding())
		{
			sprite.enabled = true;
			col.enabled = true;
		}

    }
}
