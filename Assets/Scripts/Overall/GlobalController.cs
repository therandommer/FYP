﻿using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEditor;
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
	bool isLoading = false;
	[SerializeField]
	bool isGameplay = false;
	[SerializeField]
	bool needsUIUpdate = false;
	[SerializeField]
	bool buildClicked = false;
	#endregion
	#region UI Objects
	[Header("UI Elements")]
	[SerializeField]
	GameObject BuildUI = null;
	[SerializeField]
	GameObject GamePlayUI = null;
	[SerializeField]
	GameObject PauseUI = null;
	[SerializeField]
	GameObject buildButton = null; //only enable this on pause AND gameplay
	[SerializeField]
	GameObject eraseButton = null; //only enable this on pause AND building
	#endregion

	#region This Object
	[SerializeField]
	private readonly Toggle toggle;
	[SerializeField]
	private readonly AudioSource backgroundAudio;
	#endregion

	#region References for saving/loading
	TotalObjects tO = null;
	BuildSettings build = null;
	PlatformingManager platform = null;
	[SerializeField]
	Pointer pointer = null;
	Background bG = null;
	string saveString = "Assets/SaveGames/";
	#endregion

	private void Awake()
	{
		bG = FindObjectOfType<Background>();
		build = FindObjectOfType<BuildSettings>();
		platform = FindObjectOfType<PlatformingManager>();
		tO = FindObjectOfType<TotalObjects>();
		pointer = FindObjectOfType<Pointer>();
		needsUIUpdate = true;
	}

	private void Start()
	{
		SaveGame("655 DON'T TOUCH"); //ensures erase will work correctly
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
			save.backgroundID = bG.GetComponent<Background>().GetBackgroundID();
			Debug.Log("Saving tile with ID: " + tile.GetID());
			i++;
		}
		//could save the limits for each block later
		return save;
	}
	public void SaveGame(string affix)
	{
		isSaving = true;
		if(!Directory.Exists (saveString))
		{
			Directory.CreateDirectory(saveString);
		}
		SaveGame save = CreateSaveGameObject(); //getting the data for the save
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(saveString + "/SaveData" + affix + ".a"); //creating file name
		Debug.Log(file);
		bf.Serialize(file, save); //mounting the data to the file
		file.Close();
		//AssetDatabase.Refresh();
		Debug.Log("Game Saved");
		isSaving = false;
	}
	public void ResetLevel() //used to clear absolutely everything
	{
		build.ResetAllObjects();
		tO.DestroyAll();
	}
	public void LoadGame(string affix)
	{
		Debug.Log("Reached load");
		//if (File.Exists(Application.persistentDataPath + "/SaveData.a")) //game save data must exist for this to read
		if (File.Exists(saveString + "SaveData" + affix + ".a"))
		{
			isLoading = true;
			List<GameObject> objects = tO.GetObjectsInScene();
			for (int i = 0; i < objects.Count; i++) //erases all placed objects in the scene
			{
				objects[i].GetComponent<BaseObject>().Erase();
			}
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(saveString + "/SaveData" + affix + ".a", FileMode.Open);
			Debug.Log("File loaded from: " + saveString + "/SaveData" + affix + ".a");
			SaveGame save = (SaveGame)bf.Deserialize(file); //creates the save object based on the data read from the file
			file.Close();

			for (int i = 0; i < save.objectPositionsX.Count; i++) //loops through objects stored
			{
				Vector3 position = new Vector3(save.objectPositionsX[i], save.objectPositionsY[i], save.objectPositionsZ[i]);
				int objectType = save.objectTypes[i];
				GameObject tmp = Instantiate(pointer.GetPlaceableObjects()[objectType], position, transform.rotation, tO.transform);
				tmp.GetComponent<BaseObject>().SetID(objectType);
				//Debug.Log("Loading id: " + objectType);
			}

			Debug.Log("Game Loaded");
			objects = tO.GetObjectsInScene();
			bG.SetBackgroundID(save.backgroundID);
			//remove pause state
			isLoading = false;
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
		else if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
		{
			SetIsPaused(false);
		}
		if (needsUIUpdate)
		{
			if (isPaused)
			{
				PauseUI.SetActive(true);
				if (!isGameplay)
				{
					buildButton.SetActive(false);
					eraseButton.SetActive(true);
				}
				if (isGameplay)
				{
					buildButton.SetActive(true);
					eraseButton.SetActive(false);
				}
				//enable pause UI as an overlay
				if (!buildClicked)
				{
					needsUIUpdate = false;
				}

			}
			if (isBuilding)
			{
				buildClicked = false;
				BuildUI.SetActive(true);
				GamePlayUI.SetActive(false);
				//disable other UI elements, enable building canvas
				needsUIUpdate = false;
			}
			if (isGameplay)
			{
				buildClicked = false;
				GamePlayUI.SetActive(true);
				BuildUI.SetActive(false);
				//disable other UI elements, enable gameplay canvas
				needsUIUpdate = false;
			}
			if (isSaving) //used for saving and loading
			{
				//save bar overlay, could be added. probably not needed
			}

		}
		if (isGameplay && !GamePlayUI.activeInHierarchy)
		{
			GamePlayUI.SetActive(true);
			BuildUI.SetActive(false);
		}
		if (isBuilding && !BuildUI.activeInHierarchy)
		{
			GamePlayUI.SetActive(false);
			BuildUI.SetActive(true);
		}
		if (isGameplay && isBuilding)
		{
			isGameplay = false;
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
	public void SetNeedsUIUpdate(bool _update)
	{
		needsUIUpdate = _update;
	}
	public bool GetIsPaused()
	{
		return isPaused;
	}
	public void SetIsSaving(bool _isSaving)
	{
		isSaving = _isSaving;
	}
	public void SetBuildClicked(bool _buildClick)
	{
		buildClicked = _buildClick;
	}
	public bool GetIsSaving()
	{
		return isSaving;
	}
	public bool GetIsLoading()
	{
		return isLoading;
	}
	public void GetIsLoading(bool _isLoading)
	{
		isLoading = _isLoading;
	}
	#endregion
}
