using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalObjects : MonoBehaviour
{
	[SerializeField]
	List<GameObject> objects = new List<GameObject>();
	void totalObjects()
	{
		for(int i = 0; i<objects.Count; i++)
		{
			objects.Remove(objects[i]);
		}
		objects = new List<GameObject>();
		int children = transform.childCount;
		foreach(Transform child in transform) //for positions
		{
			objects.Add(child.gameObject);
		}
	}

	public List<GameObject> GetObjectsInScene()
	{
		totalObjects();
		return objects;
	}

	public void DestroyAll()
	{
		for(int i = 0; i<objects.Count; i++)
		{
			objects[i].GetComponent<BaseObject>().Erase();
		}
	}
}
