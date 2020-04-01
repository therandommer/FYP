using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlatformingManager : MonoBehaviour
{
	#region Gameplay
	[SerializeField]
	float timeLeft = 0.0f;
	[SerializeField]
	int roundedTime = 0;
	[SerializeField]
	int coins = 0;
	[SerializeField]
	int score = 0;
	bool needPlayer = true; //allows proper linking to the player
	bool needReset = true; //determines when the platforming stats need resetting
	bool hasReset = false; //limits to 1 reset
	bool hasInitialised = false;
	#endregion

	#region References
	[SerializeField]
	GlobalController gc;
	[SerializeField]
	BuildSettings build;
	[SerializeField]
	Player player;
	#endregion

	#region UI
	[SerializeField]
	TextMeshProUGUI scoreText;
	[SerializeField]
	TextMeshProUGUI coinText;
	[SerializeField]
	TextMeshProUGUI timeText;
	[SerializeField]
	TextMeshProUGUI healthText; //may end up changing to a bar or other graphic
	#endregion

	void Awake()
    {
		timeLeft = build.GetTime();
    }

    void Update()
    {
		if (gc.GetIsGameplay()) //gameplay specific in here
		{
			if(hasReset) //allows for a reset when the player goes to build
			{
				hasReset = false;
			}
			if (build.GetPlayerSpawned() && needPlayer) //initialises the player this loop
			{
				player = FindObjectOfType<Player>();
				needPlayer = false;
			}
			if(!hasInitialised) //initalises the values of the ui on play
			{
				coinText.text = "Coins: " + coins;
				scoreText.text = "Score: " + score;
				healthText.text = "Health: " + player.GetComponent<Player>().GetMaxHealth();
				hasInitialised = true;
			}
			
            timeLeft -= Time.deltaTime;
			roundedTime = Mathf.FloorToInt(timeLeft);
			timeText.text = "Time: " + roundedTime;
		}
		if (gc.GetIsBuilding() && needReset && !hasReset) //resets, etc. here
		{
			ResetPlatformStats();
		} 
    }

    #region setters and getters
    public void IncrementCoins(int _amount)
    {
        coins += _amount;
		coinText.text = "Coins: " + coins;
    }
    public int GetCoins()
    {
        return coins;
    }
    public void IncrementScore(int _amount)
    {
        score += _amount;
		scoreText.text = "Score: " + score;
    }
    public int GetScore()
    {
        return score;
    }
    public void ResetTime()
    {
        timeLeft = build.GetTime();
    }
	public void ResetPlatformStats()
	{
		needPlayer = true;
		needReset = false;
		hasReset = true;
		hasInitialised = false;
		timeLeft = 0.0f;
		coins = 0;
		score = 0;
	}
	public bool GetHasReset()
	{
		return hasReset;
	}
    #endregion
}
