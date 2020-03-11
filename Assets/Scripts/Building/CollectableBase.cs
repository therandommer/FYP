using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBase : MonoBehaviour
{
	[SerializeField]
	[Range(1, 3)]
	int collectableType = 1;
	PlatformingManager platform;

	private void Awake()
	{
		platform = FindObjectOfType<PlatformingManager>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			CollectThis();
		}
	}
	void CollectThis()
	{
		switch (collectableType)
		{
			case 1: //coin
				break;
			case 2: //powerup
				break;
			case 3:
				break;
			default:
				break;
		}
	}
	void Update()
    {
        
    }
}
