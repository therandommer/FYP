using UnityEngine;
using UnityEngine.UI;

public class UIBaseObject : MonoBehaviour
{
	[SerializeField]
	readonly int objectID;

	Image childImage;

	private void Start()
	{
		childImage = gameObject.GetComponentInChildren<Image>();
	}

	public Image GetImage()
	{
		return childImage;
	}
}
