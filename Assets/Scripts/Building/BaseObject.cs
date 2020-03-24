using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    [SerializeField] [Range(1,6)] //object type number is the same as buildsettings
    int objectType = 0;
    [SerializeField]
    int objectValue = 1;
	[SerializeField]
	int thisID = 0;
    [SerializeField]
    Vector3 defaultPosition;
    [SerializeField]
    BuildSettings build;
	[SerializeField]
	Pointer pointer;
	[SerializeField]
	TotalObjects parentObject;

    void Awake()
    {
		parentObject = FindObjectOfType<TotalObjects>();
		pointer = FindObjectOfType<Pointer>();
		thisID = pointer.GetHeldID();
		build = FindObjectOfType<BuildSettings>();
        if(build.GetCurrentObjects(objectType) + objectValue <= build.GetMaxObjects(objectType)) //used to limit and track number of objects placed. Mainly used to limit player to 1
        {
            build.IncrementObject(objectType, objectValue);
        }
        else
        {
            Erase();
        }
        defaultPosition = this.transform.position;
		if (defaultPosition.x < build.GetLevelSmallest().x || defaultPosition.y < build.GetLevelSmallest().y)
		{
			Erase();
		}
		if (defaultPosition.x > build.GetLevelLimit().x || defaultPosition.y > build.GetLevelLimit().y)
		{
			Erase();
		}
	}
    //will be called when player returns from gameplay to building
    private void Reset()
    {
        this.transform.position = defaultPosition;
    }
    public void Erase()
    {
		//erase from arrays and whatnot
		build.IncrementObject(objectType, -objectValue); 
        Destroy(this.gameObject);
    }
	public Vector3 GetDefaultPosition() //in case an object has moved from its original spot, called when creating saves
	{
		return defaultPosition;
	}
	public int GetID()
	{
		return thisID;
	}
	public void SetID(int _id)
	{
		thisID = _id;
	}
}
