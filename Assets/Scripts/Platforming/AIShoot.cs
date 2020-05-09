using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShoot : MonoBehaviour
{
	[SerializeField]
	float interval = 2.0f;
	float currentTimer = 0.0f;
	GlobalController gc = null;
	[SerializeField]
	GameObject bullet = null;
	private void Awake()
	{
		gc = FindObjectOfType<GlobalController>();
	}
	void Shoot()
	{
		Debug.Log("AI shot");
		Instantiate(bullet, transform.position, transform.rotation, gameObject.transform.Find("LevelObjects"));
	}
	void Update()
    {
		if (gc.GetIsGameplay() && !gc.GetIsPaused() && currentTimer <= 0.0f)
		{
			currentTimer = interval;
			Shoot();
		}
		if(currentTimer > 0.0f)
		{
			currentTimer -= Time.deltaTime;
		}
		if(gc.GetIsBuilding())
		{
			currentTimer = 0.0f;
		}
    }
}
