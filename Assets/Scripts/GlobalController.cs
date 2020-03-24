﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This script keeps a track of all the states that the game will go through.
/// Also keeps track of some of the options the player can make, turning music on/off, etc.
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
	#region UI Objects
	[Header("UI Elements")]
	[SerializeField]
	GameObject BuildUI;
	[SerializeField]
	GameObject GamePlayUI;
	[SerializeField]
	GameObject PauseUI;
	[SerializeField]
	Button buildButton; //only enable this on pause AND gameplay
	#endregion

	#region This Object
	[SerializeField]
	private Toggle toggle;
	[SerializeField]
	private AudioSource backgroundAudio;
	#endregion

	#region References for saving/loading
	TotalObjects tO;
	BuildSettings build;
	[SerializeField]
	Pointer pointer;
	#endregion

	private void Awake()
	{
		build = FindObjectOfType<BuildSettings>();
		tO = FindObjectOfType<TotalObjects>();
		pointer = FindObjectOfType<Pointer>();
		needsUIUpdate = true;
	}
	private SaveGame CreateSaveGameObject() //Currently saves anything with "BaseObject" attached to it
	{
		List<GameObject> Objects = tO.GetObjectsInScene();
		SaveGame save = new SaveGame();
		int i = 0;
		foreach (GameObject tileObject in Objects)
		{
			BaseObject tile = tileObject.GetComponent<BaseObject>();
			save.objectPositionsX.Add(tile.GetDefaultPosition().x); //default position
			save.objectPositionsY.Add(tile.GetDefaultPosition().y);
			save.objectPositionsZ.Add(tile.GetDefaultPosition().z);
			save.objectTypes.Add(tile.GetID()); //Object Id for instantiating
			i++;
		}
		//could save the limits for each block later
		return save;
	}
	public void SaveGame()
	{
		isSaving = true;
		SaveGame save = CreateSaveGameObject(); //getting the data for the save
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/SaveData.a"); //creating file name
		bf.Serialize(file, save); //mounting the data to the file
		file.Close();

		///can do some cleanup if needed, probably display something to the player?
		Debug.Log("Game Saved");
		isSaving = false;
	}
	public void ResetLevel() //used to clear absolutely everything
	{
		build.ResetAllObjects();
		tO.DestroyAll();
	}
	public void LoadGame()
	{
		if (File.Exists(Application.persistentDataPath + "/SaveData.a")) //game save data must exist for this to read
		{
			isSaving = true;
			//ResetLevel(); //resets the level to a blank state
			List<GameObject> objects = tO.GetObjectsInScene();
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/SaveData.a", FileMode.Open);
			SaveGame save = (SaveGame)bf.Deserialize(file); //creates the save object based on the data read from the file
			file.Close();

			for (int i = 0; i < save.objectPositionsX.Count; i++) //loops through objects stored
			{
				Vector3 position = new Vector3(save.objectPositionsX[i], save.objectPositionsY[i], save.objectPositionsZ[i]);
				int objectType = save.objectTypes[i];
				GameObject tmp = Instantiate(pointer.GetPlaceableObjects()[objectType], position, transform.rotation, tO.transform);
			}

			//remove graphic or something

			Debug.Log("Game Loaded");

			//remove pause state
			isSaving = false;
			isPaused = false;
		}
		else
		{
			Debug.Log("No game saved!");
		}
	}
	public void QuitGame()
	{
		Application.Quit();
	}
	void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
		{
			SetIsPaused(true);
		}
		else if(Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
		{
			SetIsPaused(false);
		}
        if(needsUIUpdate)
		{
			if(isPaused)
			{
				PauseUI.SetActive(true);
				if(!isGameplay)
				{
					buildButton.enabled = false;
				}
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
				//disable other UI elements, enable gameplay canvas
				needsUIUpdate = false;
			}
			if(isSaving) //used for saving and loading I think
			{
				//save bar overlay
			}
			
		}
		if (!isPaused && PauseUI.activeInHierarchy)
		{
			PauseUI.SetActive(false);
			needsUIUpdate = false;
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
