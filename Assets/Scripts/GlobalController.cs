using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
	#region States
	[Header("States")]
	[SerializeField]
	bool isPaused = false;
	[SerializeField]
	bool isBuilding = true; //when true building UI enabled, when false playing level.
	[SerializeField]
	bool isSaving = false;
<<<<<<< Updated upstream
=======
	[SerializeField]
	bool isGameplay = false;
	[SerializeField]
	bool needsUIUpdate = false;
	[SerializeField]
	bool needPlayerSpawn = false;
>>>>>>> Stashed changes
	#endregion
	#region UI Elements
	[Header("UI Elements")]
	[SerializeField]
	GameObject BuildUI;
	[SerializeField]
	GameObject GamePlayUI;
	[SerializeField]
	GameObject OtherUI;
	#endregion

	void Start()
    {
        
    }

    void Update()
    {
        
    }
}
