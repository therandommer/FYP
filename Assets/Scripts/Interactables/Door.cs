using UnityEngine;

public class Door : MonoBehaviour
{
	/*[SerializeField]
	int linkNo = 1; //used to change where each door links to. 1-1 2-2, etc.
	[SerializeField]
	bool isAddedToList = false;
	[SerializeField]
	bool isLinkedToDoor = false;
    [SerializeField]
    TextMeshPro linkText = null;
	[SerializeField]
	Vector2 otherDoor;
	[SerializeField]
	BuildSettings buildSettings = null;
	[SerializeField]
	InteractableBaseObject thisBaseObject = null;

	private void Awake()
	{
		buildSettings = FindObjectOfType<BuildSettings>();
		buildSettings.CheckLinks();
		buildSettings.AddToDoors(this.gameObject);
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player" && Input.GetKeyDown(KeyCode.E))
		{
			collision.transform.position = otherDoor;
		}
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Player" && Input.GetKeyDown(KeyCode.E))
		{
			collision.transform.position = otherDoor;
		}
	}

	void Update()
    {
		//linking to other doors 1-1, 2-2, etc.
        if(!isLinkedToDoor)
		{
			Debug.Log("actually reaching other door");
			if(buildSettings.GetDoorList().Count>1)
			{
				for(int i = 0; i<buildSettings.GetDoorList().Count; i++)
				{
					if (buildSettings.GetDoorList()[i].GetComponent<Door>().GetLinkNo() == linkNo && buildSettings.GetDoorList()[i].transform.position != transform.position)
					{
						otherDoor = buildSettings.GetDoorList()[i].transform.position;
						isLinkedToDoor = true;
						Debug.Log(buildSettings.GetDoorList()[i].transform.position);
					}
				}
			}
		}
    }

	public void SetDoorlinkNo(int _id)
	{
		linkNo = _id;
        linkText.text = _id.ToString();
		thisBaseObject.HideElements();
		otherDoor = Vector2.zero;
		isLinkedToDoor = false;
	}
	public int GetLinkNo()
	{
		return linkNo;
	} */
}
