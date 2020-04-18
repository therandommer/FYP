using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
	[SerializeField]
	private List<Sprite> backgrounds = new List<Sprite>(); //holds potential backgrounds for player to choose
	int activeBackground = 0;
	[SerializeField]
	SpriteRenderer sprite = null;
	[SerializeField]
	Slider backgroundSlider = null;
	[SerializeField]
	Text backgroundText = null;
	void Start()
	{
		sprite = gameObject.GetComponent<SpriteRenderer>();
	}
	private void Awake()
	{
		backgroundText.text = "Background: " + ((int)backgroundSlider.value + 1);
	}
	private void Update()
	{
		if (activeBackground != backgroundSlider.value)
		{
			UpdateBackground();
		}
	}
	private void UpdateBackground()
	{
		activeBackground = (int)backgroundSlider.value;
		sprite.sprite = backgrounds[(int)backgroundSlider.value];
		backgroundText.text = "Background: " + ((int)backgroundSlider.value + 1);
	}
	public int GetBackgroundID()
	{
		return activeBackground;
	}
	public void SetBackgroundID(int _id) //only called on load
	{
		activeBackground = _id;
		backgroundSlider.value = _id;
		UpdateBackground();
	}
}
