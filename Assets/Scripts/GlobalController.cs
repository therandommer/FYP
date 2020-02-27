using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script keeps a track of all the states that the game will go through.
/// </summary>
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
	[SerializeField]
	bool isGameplay = false;
	[SerializeField]
	bool needsUIUpdate = false;
	#endregion
	#region UI Elements
	[Header("UI Elements")]
	[SerializeField]
	GameObject BuildUI;
	[SerializeField]
	GameObject GamePlayUI;
	[SerializeField]
	GameObject PauseUI;
	#endregion

	void Start()
    {
        
    }

    void Update()
    {
        if(needsUIUpdate)
		{
			if(isPaused)
			{
				PauseUI.SetActive(true);
				//enable pause UI as an overlay
				needsUIUpdate = false;
			}
			if(isBuilding)
			{
				BuildUI.SetActive(true);
				GamePlayUI.SetActive(false);
				//disable other UI elements, enable building canvas
				needsUIUpdate = false;
			}
			if(isGameplay)
			{
				GamePlayUI.SetActive(true);
				BuildUI.SetActive(false);
				//disable otehr UI elements, enable gameplay canvas
				needsUIUpdate = false;
			}
			if(isSaving)
			{
				//save bar overlay
			}
			if(!isPaused && PauseUI.activeInHierarchy)
			{
				PauseUI.SetActive(false);
				needsUIUpdate = false;
			}
		}
    }

	#region Setters and Getters
	public void SetIsBuilding(bool _isBuilding)
	{
		isBuilding = _isBuilding;
		needsUIUpdate = true;
	}
	public bool GetIsBuilding()
	{
		return isBuilding;
	}
	public void SetIsGameplay(bool _isGameplay)
	{
		isGameplay = _isGameplay;
		needsUIUpdate = true;
	}
	public bool GetIsGameplay()
	{
		return isGameplay;
	}
	public void SetIsPaused(bool _isPaused)
	{
		isPaused = _isPaused;
		needsUIUpdate = true;
	}
	public bool GetIsPaused()
	{
		return isPaused;
	}
	public void SetIsSaving(bool _isSaving)
	{
		isSaving = _isSaving;
	}
	public bool GetIsSaving()
	{
		return isSaving;
	}
	#endregion
}
