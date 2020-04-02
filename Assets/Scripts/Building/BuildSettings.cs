using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This script keeps various values for gameplay stored in a convenient location.
/// Also this script limits the number of tiles/entities to be placed
/// </summary>
public class BuildSettings : MonoBehaviour
{
	#region Level Variables
	[Header("Level Variables")]
	[Space(10)]
	[SerializeField]
	float time = 200.0f; //this will need to be rounded for the UI display
	[SerializeField]
	Vector2 playerSpawnPoint; //on play initialise player to this location
	[SerializeField]
	bool playerSpawned = false;
	[Header("Level Limiters")]
	[SerializeField]
	Vector2 levelLimits; //will need to set min and max to this
	Vector2 minBaseLevelLimits = new Vector2Int(0,0);
	Vector2 maxBaseLevelLimits = new Vector2Int(512,256);
	Vector2 maxModifiedLevelLimits = new Vector2Int(512,256); //when player determines how long the level is
	[SerializeField]
	int placedTerrain = 0;
	int maxTerrain = 2048;
	[SerializeField]
	int placedFriendly = 0;
	int maxFriendly = 256;
	[SerializeField]
	int placedHazards = 0;
	int maxHazards = 512;
	[SerializeField]
	int placedInteractables = 0;
	int maxInteractables = 1024;
	[SerializeField]
	int placedOther = 0;
	int maxOther = 1024;
    [SerializeField]
    int placedPlayer = 0;
	[SerializeField]
	int maxPlayer = 1;
	[SerializeField]
	int placedExit = 0;
	[SerializeField]
	int maxExit = 1;
    [SerializeField]
    int placedDoors = 0;
    int maxDoors = 10;
	//door links
	[SerializeField]
	bool isLink1Available = true;
	[SerializeField]
	bool isLink2Available = true;
	[SerializeField]
	bool isLink3Available = true;
	[SerializeField]
	bool isLink4Available = true;
	[SerializeField]
	bool isLink5Available = true;
	[Space(10)]
	[SerializeField]
	bool isClearConditionEnabled = false; //if true then restrict end point access till following completed
	[SerializeField]
	GameObject clearObject; //need a reference to a type of object, so they can be taken when the level is played.
	[SerializeField]
	int clearConditionRequired = 0; //this will probably change
	#endregion

	#region User Settings
	[Header("User Settings")]
	[Space(10)]
	List<GameObject> Favourites = new List<GameObject>(); //save the player prefered tiles here
	#endregion

	#region Object lists/error UI
	[SerializeField]
	List<GameObject> Doors; //used to link positions
	[SerializeField]
	Player player; //hold the reference to the player object
	[SerializeField]
	GlobalController gc;
	[SerializeField]
	TextMeshProUGUI errorText;
	[SerializeField]
	Button playerSpawnButton;
	[SerializeField]
	Button exitButton;
	#endregion

	#region Setters and Getters
	public void IncrementObject(int _type,int _amount)
	{
		switch(_type)
		{
			case 1:
                placedTerrain += _amount;
                break;
			case 2:
				placedFriendly += _amount;
				break;
			case 3:
				placedHazards += _amount;
				break;
			case 4:
				placedInteractables += _amount;
				break;
			case 5:
				placedOther += _amount;
				break;
            case 6:
                placedPlayer += _amount;
				Debug.Log(placedPlayer);
                break;
            case 7:
                placedExit += _amount;
				Debug.Log(placedExit);
                break;
			/*case 8:
				placedDoors += _amount;
				break;*/
			default:
				break;
		}
	}
    public int GetMaxObjects(int _type)
    {
        switch(_type)
        {
            case 1:
                return maxTerrain;
            case 2:
                return maxFriendly;
            case 3:
                return maxHazards;
            case 4:
                return maxInteractables;
            case 5:
                return maxOther;
            case 6:
                return maxPlayer;
			case 7:
				return maxExit;
            default:
                break;
        }
        Debug.Log("Object incorrectly defined");
        return 0;
    }
    public int GetCurrentObjects(int _type)
    {
        switch(_type)
        {
            case 1:
                return placedTerrain;
            case 2:
                return placedFriendly;
            case 3:
                return placedHazards;
            case 4:
                return placedInteractables;
            case 5:
                return placedOther;
            case 6:
                return placedPlayer;
			case 7:
				return maxExit;
			default:
				break;
        }
        Debug.Log("Object incorrectly defined");
        return 0;
    }
	public void ResetAllObjects()
	{
		placedTerrain = 0;
		placedFriendly = 0;
		placedHazards = 0;
		placedInteractables = 0;
		placedOther = 0;
		placedPlayer = 0;
		Doors = new List<GameObject>();
		playerSpawned = false;
		time = 200.0f;
		/*bool isLink1Available = true;
		bool isLink2Available = true;
		bool isLink3Available = true;
		bool isLink4Available = true;
		bool isLink5Available = true;*/
}
	public void SetPlayerSpawn(Vector2 _newSpawn)
	{
		if(_newSpawn.x > minBaseLevelLimits.x && _newSpawn.y > minBaseLevelLimits.y &&
			_newSpawn.x < maxBaseLevelLimits.x && _newSpawn.y < maxBaseLevelLimits.y)
		{
			playerSpawnPoint = _newSpawn;
		}
	}
	public Vector2 GetPlayerSpawn()
	{
		return playerSpawnPoint;
	}
	public void SetLevelLimitX(int _x)
	{
		if(_x<minBaseLevelLimits.x)
		{
			levelLimits.x = minBaseLevelLimits.x;
		}
		if(_x>maxBaseLevelLimits.x)
		{
			levelLimits.x = maxBaseLevelLimits.x;
		}
		else
		{
			levelLimits.x = _x;
		}
	}
	public void SetLevelLimitY(int _y)
	{
		if (_y < minBaseLevelLimits.y)
		{
			levelLimits.y = minBaseLevelLimits.y;
		}
		if (_y > maxBaseLevelLimits.y)
		{
			levelLimits.y = maxBaseLevelLimits.y;
		}
		else
		{
			levelLimits.x = _y;
		}
	}
	public Vector2 GetLevelLimit()
	{
		return maxBaseLevelLimits;
	}
	public Vector2 GetLevelSmallest()
	{
		return minBaseLevelLimits;
	}
	public void SetPlayerSpawend(bool _spawned)
	{
		playerSpawned = _spawned;
	}
	public bool GetPlayerSpawned()
	{
		return playerSpawned;
	}
	public List<GameObject> GetDoorList()
	{
		return Doors;
	}
	public void AddToDoors(GameObject _door)
	{
		Doors.Add(_door);
	}
	public void SetTime(float _time)
	{
		time = _time;
	}
	public float GetTime()
	{
		return time;
	}

	#endregion

	void Awake()
    {
		errorText.enabled = false;
    }

    void Update()
    {
		if(!gc.GetIsBuilding() && placedPlayer != maxPlayer) //player required first
		{
			errorText.text = "Need to place a player to play!";
			gc.SetIsBuilding(true);
		}
		if(!gc.GetIsBuilding() && placedExit != maxExit) //exit required next
		{
			errorText.text = "Need to place an exit to play!";
			gc.SetIsBuilding(true);
		}
		if(!gc.GetIsBuilding() && placedPlayer == maxPlayer && !GetPlayerSpawned()) //sets initial player spawn point
		{
			Vector3 tmp = new Vector3(playerSpawnPoint.x, playerSpawnPoint.y, 0);
			Instantiate(player, tmp,this.transform.rotation,this.gameObject.transform);
			playerSpawned = true;
		}
		if(gc.GetIsBuilding() && placedPlayer == maxPlayer && playerSpawnButton.enabled == true)
		{
			playerSpawnButton.enabled = false;
		}
		else if(gc.GetIsBuilding() && placedPlayer != maxPlayer && playerSpawnButton.enabled == false)
		{
			playerSpawnButton.enabled = true;
		}
		if(gc.GetIsBuilding() && placedExit == maxExit && exitButton.enabled == true)
		{
			exitButton.enabled = false;
		}
		else if (gc.GetIsBuilding() && placedExit != maxExit && exitButton.enabled == false)
		{
			exitButton.enabled = true;
		}
		if (gc.GetIsBuilding() && GetPlayerSpawned())
		{
			Destroy(GameObject.Find("Player"));
			playerSpawned = false;
		}
	}
    ///not used right now
    #region doors 
    //setting which links can be used by the player, used to set the default links for each object
    public void CheckLinks()
	{
		int tmp1 = 0;
		int tmp2 = 0;
		int tmp3 = 0;
		int tmp4 = 0;
		int tmp5 = 0;
		for (int i = 0; i < GetDoorList().Count; i++)
		{
			switch (GetDoorList()[i].GetComponent<Door>().GetLinkNo())
			{
				case 1:
					tmp1++;
					if (tmp1 > 2)
					{
						isLink1Available = false;

					}
					break;
				case 2:
					tmp2++;
					if (tmp2 > 2)
					{
						isLink2Available = false;

					}
					break;
				case 3:
					tmp3++;
					if (tmp3 > 2)
					{
						isLink3Available = false;

					}
					break;
				case 4:
					tmp4++;
					if (tmp4 > 2)
					{
						isLink4Available = false;

					}
					break;
				case 5:
					tmp5++;
					if (tmp5 > 2)
					{
						isLink5Available = false;

					}
					break;
				default:
					break;
			}
			if(isLink1Available)
			{
				GetDoorList()[i].GetComponent<Door>().SetDoorlinkNo(1);
			}
			if (isLink2Available)
			{
				GetDoorList()[i].GetComponent<Door>().SetDoorlinkNo(2);
			}
			if (isLink3Available)
			{
				GetDoorList()[i].GetComponent<Door>().SetDoorlinkNo(3);
			}
			if (isLink4Available)
			{
				GetDoorList()[i].GetComponent<Door>().SetDoorlinkNo(4);
			}
			if (isLink5Available)
			{
				GetDoorList()[i].GetComponent<Door>().SetDoorlinkNo(5);
			}
		}
	}
	#endregion //not used right now, keeping for 
}
