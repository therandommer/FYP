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
	bool hasInitialised = false;
	#endregion

	#region References
	[SerializeField]
	GlobalController gc = null;
	[SerializeField]
	BuildSettings build = null;
	[SerializeField]
	Player player = null;
	#endregion

	#region UI
	[SerializeField]
	TextMeshProUGUI scoreText = null;
	[SerializeField]
	TextMeshProUGUI coinText = null;
	[SerializeField]
	TextMeshProUGUI timeText = null;
	[SerializeField]
	TextMeshProUGUI healthText = null; //may end up changing to a bar or other graphic
	#endregion

	void Awake()
    {
		timeLeft = build.GetTime();
    }

    void Update()
    {
		if (gc.GetIsGameplay()) //gameplay specific in here
		{
			if(!needReset) //allows for a reset when the player goes to gameplay
			{
				needReset = true;
			}
			if (build.GetPlayerSpawned() && needPlayer) //initialises the player this loop
			{
				Debug.Log("Initialising player(Platforming)");
				player = FindObjectOfType<Player>();
				needPlayer = false;
			}
			if(!hasInitialised) //initalises values for this script on play
			{
				player = null;
				timeLeft = build.GetTime();
				timeText.text = "Time: " + timeLeft;
				coinText.text = "Coins: " + coins;
				scoreText.text = "Score: " + score;
				hasInitialised = true;
			}
			if(player == null)
			{
				player = FindObjectOfType<Player>();
			}
			timeLeft -= Time.deltaTime;
			roundedTime = Mathf.FloorToInt(timeLeft);
			if(player != null)
			{
				UpdateUI();
			}
		}
		if (gc.GetIsBuilding() && needReset) //resets, etc. here when entering building
		{
			ResetPlatformStats();
		} 
    }

    #region setters and getters
	public void UpdateUI()
	{
		coinText.text = "Coins: " + coins;
		scoreText.text = "Score: " + score;
		timeText.text = "Time: " + roundedTime;
		healthText.text = "Health: " + player.GetComponent<Player>().GetHealth() + "/" + player.GetComponent<Player>().GetMaxHealth();
	}
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
	public void ResetPlatformStats()
	{
		player = null;
		needPlayer = true;
		needReset = false;
		hasInitialised = false;
		timeLeft = build.GetTime();
		coins = 0;
		score = 0;
	}
	public bool GetHasReset()
	{
		return needReset;
	}
    #endregion
}
