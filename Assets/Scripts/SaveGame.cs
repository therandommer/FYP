using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //need this for the save functionality
public class SaveGame //no monobehabiour inheritance
{
	public List<float> objectPositionsX = new List<float>(); //used to respawn every tile in the correct position
	public List<float> objectPositionsY = new List<float>();
	public List<float> objectPositionsZ = new List<float>();
	public List<int> objectTypes = new List<int>();
	public int backgroundID = 0;
}
