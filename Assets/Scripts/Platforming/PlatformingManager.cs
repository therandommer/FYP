using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingManager : MonoBehaviour
{
	/// Gameplay
	[SerializeField]
	float timeLeft = 0.0f;
	[SerializeField]
	int coins = 0;
	[SerializeField]
	int score = 0;

	///references
	[SerializeField]
	GlobalController gc;
	[SerializeField]
	BuildSettings build;
	[SerializeField]
	Player player;
	bool needPlayer = true;
	bool needReset = true;
    void Awake()
    {
		timeLeft = build.GetTime();
    }

    // Update is called once per frame
    void Update()
    {
		if (gc.GetIsGameplay()) //gameplay specific in here
		{
			if (build.GetPlayerSpawned() && needPlayer)
			{
				player = FindObjectOfType<Player>();
				needPlayer = false;
			}
		}
		if (gc.GetIsBuilding() && needReset) //resets, etc. here
		{
			needPlayer = true;
			needReset = false;
			timeLeft = 0.0f;
		} 
    }
}
