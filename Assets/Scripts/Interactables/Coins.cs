using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
	BuildSettings build = null;
	[SerializeField]
	SpriteRenderer sprite = null;
	[SerializeField]
	BoxCollider2D col;
	GlobalController gc;
	PlatformingManager platform;

	[SerializeField]
	bool hasCollected = false;
	[SerializeField]
	bool isSpecial = false; //gold coins are special, red aren't

	// Start is called before the first frame update
	void Start()
    {
		build = FindObjectOfType<BuildSettings>();
		platform = FindObjectOfType<PlatformingManager>();
		gc = FindObjectOfType<GlobalController>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player")
		{
			hasCollected = true;
			if(!isSpecial)
			{
				platform.IncrementCoins(1);
				platform.IncrementScore(100);
			}
			else if(isSpecial)
			{
				platform.IncrementCoins(2);
				platform.IncrementScore(500);
			}
		}
	}

	// Update is called once per frame
	void Update()
    { 
		if (!gc.GetIsBuilding() && hasCollected) //disables object when collected
		{
			sprite.enabled = false;
			col.enabled = false;
		}
		else if (gc.GetIsBuilding()) //re-enables in the build screen
		{
			hasCollected = false;
			sprite.enabled = true;
			col.enabled = true;
		}
	}
}
