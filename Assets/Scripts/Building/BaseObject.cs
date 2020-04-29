using UnityEngine;

public class BaseObject : MonoBehaviour
{
	[SerializeField]
	[Range(1, 7)] //1-terrain, 2-Friendly, 3-Hazards, 4-Interactables, 5-Other, 6-Player, 7-Exit
	private int objectType = 0;
	[SerializeField]
	private int objectValue = 1;
	[SerializeField]
	int thisID = 0;
	[SerializeField]
	Vector3 defaultPosition = new Vector3(0, 0, 0);
	[SerializeField]
	BuildSettings build = null;
	[SerializeField]
	Pointer pointer = null;
	[SerializeField]
	TotalObjects parentObject = null;
	GlobalController gc = null;

	void Awake()
	{
		gc = FindObjectOfType<GlobalController>();
		parentObject = FindObjectOfType<TotalObjects>();
		pointer = FindObjectOfType<Pointer>();
		if (!gc.GetIsLoading())
		{
			SetID(pointer.GetHeldID());
		}
		build = FindObjectOfType<BuildSettings>();

		if (objectType <= 5)
		{
			if (build.GetCurrentObjects(objectType) + objectValue <= build.GetMaxObjects(objectType)) //used to limit and track number of objects placed. Mainly used to limit player to 1
			{
				build.IncrementObject(objectType, objectValue);
			}
			if (build.GetCurrentObjects(objectType) + objectValue > build.GetMaxObjects(objectType))
			{
				Debug.Log(build.GetCurrentObjects(objectType));
				Erase();
			}
		}
		if (objectType >= 6)
		{
			if (build.GetCurrentObjects(objectType) <= build.GetMaxObjects(objectType)) //used to limit and track number of objects placed. Mainly used to limit player to 1
			{
				build.IncrementObject(objectType, objectValue);
			}
			if (build.GetCurrentObjects(objectType) > build.GetMaxObjects(objectType))
			{
				Debug.Log(build.GetCurrentObjects(objectType));
				Erase();
			}
		}
		defaultPosition = transform.position;
		//ensuring object is placed within bounds
		if (defaultPosition.x < build.GetLevelSmallest().x || defaultPosition.y < build.GetLevelSmallest().y)
		{
			Erase();
		}
		if (defaultPosition.x > build.GetLevelLimit().x || defaultPosition.y > build.GetLevelLimit().y)
		{
			Erase();
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (gc.GetIsBuilding())
		{
			collision.gameObject.GetComponent<BaseObject>().Erase();
		}
	}
	//will be called when player returns from gameplay to building
	private void Reset()
	{
		transform.position = defaultPosition;
	}
	public void Erase()
	{
		//erase from arrays and whatnot
		Debug.Log("Erase called");
		build.IncrementObject(objectType, -objectValue);
		Destroy(gameObject);
	}
	public Vector3 GetDefaultPosition() //in case an object has moved from its original spot, called when creating saves
	{
		return defaultPosition;
	}
	public int GetObjectType()
	{
		return objectType;
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
