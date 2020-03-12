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

    void Update()
    {
		if (gc.GetIsGameplay()) //gameplay specific in here
		{
			if (build.GetPlayerSpawned() && needPlayer)
			{
				player = FindObjectOfType<Player>();
				needPlayer = false;
			}
            timeLeft -= Time.deltaTime;
		}
		if (gc.GetIsBuilding() && needReset) //resets, etc. here
		{
			needPlayer = true;
			needReset = false;
			timeLeft = 0.0f;
		} 
    }

    #region setters and getters
    public void IncrementCoins(int _amount)
    {
        coins += _amount;
    }
    public int GetCoins()
    {
        return coins;
    }
    public void IncrementScore(int _amount)
    {
        score += _amount;
    }
    public int GetScore()
    {
        return score;
    }
    public void ResetTime()
    {
        timeLeft = build.GetTime();
    }
    #endregion
}
