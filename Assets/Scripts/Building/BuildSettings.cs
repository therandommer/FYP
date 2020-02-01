using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSettings : MonoBehaviour
{
	#region Level Variables
	[Header("Level Variables")]
	[Space(10)]
	[SerializeField]
	float time = 200.0f; //this will need to be rounded for the UI display
	[SerializeField]
	Vector2 playerSpawnPoint; //on play initialise player to this location
	[Header("Level Limiters")]
	[SerializeField]
	Vector2 levelLimits; //will need to set min and max to this
	Vector2 minBaseLevelLimits = new Vector2Int(0,0);
	Vector2 maxBaseLevelLimits = new Vector2Int(128,32);
	Vector2 maxModifiedLevelLimits = new Vector2Int(512,256); //when player determines how long the level is
	[SerializeField]
	int placedTerrain = 0;
	int maxTerrain = 2048;
	[SerializeField]
	int placedFriendly = 0;
	int maxFriendly = 256;
	[SerializeField]
	int placedHazards = 0;
	int maxHazards = 256;
	[SerializeField]
	int placedInteractables = 0;
	int maxInteractables = 1024;
	[SerializeField]
	int placedOther = 0;
	int maxOther = 1024;
	[Space(10)]
	[SerializeField]
	bool isClearConditionEnabled = false; //if true then restrict end point access till following completed
	[SerializeField]
	GameObject clearObject; //need a reference to a type of object, so they can be taken when the level is played.
	[SerializeField]
	int clearConditionNumber = 0; //this will probably change
	[SerializeField]
	bool isPlayerValid = false; //if the player is placed in a valid location then this will become true;
	#endregion

	#region User Settings
	[Header("User Settings")]
	[Space(10)]
	List<GameObject> Favourites = new List<GameObject>(); //save the player prefered tiles here
	#endregion

	void Start()
    {
        
    }

    void Update()
    {
        
    }
}
