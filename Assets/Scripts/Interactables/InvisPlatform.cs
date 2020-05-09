using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisPlatform : MonoBehaviour
{
	GlobalController gc = null;
	float fadeTime = 0.25f; //Used for the lerp of the alpha channel.
	[SerializeField]
	bool hasFaded = false; //while true is deactivated, while false is activated.
	BoxCollider2D box = null;
	SpriteRenderer thisSprite = null;
	[SerializeField]
	Color defaultColour;

    void Awake()
    {
		gc = FindObjectOfType<GlobalController>();
		box = GetComponent<BoxCollider2D>();
		thisSprite = GetComponent<SpriteRenderer>();
		defaultColour = new Color(thisSprite.color.r, thisSprite.color.g, thisSprite.color.b, thisSprite.color.a);
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player" && !hasFaded) //could allow for anything player fires to trigger them too
		{
			Debug.Log("Player found");
			hasFaded = true;
			box.enabled = false;
			StartCoroutine(FadeTo(0.0f, fadeTime));
		}
		if(collision.gameObject.tag == "Player" && hasFaded)
		{
			thisSprite.color = new Color(defaultColour.r, defaultColour.g, defaultColour.b, 0);
		}
	}
	//will fade in given time to specified alpha value; 
	IEnumerator FadeTo(float _endValue, float _fadeTimer)
	{
		float tmp = thisSprite.color.a;
		for(float t = 0.0f; t < 1.0f; t += Time.deltaTime / _fadeTimer)
		{
			Color tmpColour = new Color(defaultColour.r, defaultColour.g, defaultColour.b, Mathf.Lerp(tmp, _endValue, t));
			thisSprite.color = tmpColour;
			yield return null; 
		}
	}
	void Update()
    {
		if (gc.GetIsBuilding() && thisSprite.color.a != defaultColour.a || hasFaded) //reset on  build
		{
			thisSprite.color = defaultColour;
			box.enabled = true;
			hasFaded = false;
		}
    }
}
