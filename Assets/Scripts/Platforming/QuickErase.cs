using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickErase : MonoBehaviour
{
	float defaultTimer = 0.1f;
	[SerializeField]
	float currentTimer = 0.0f;
	[SerializeField]
	bool hasDisabled = false;
	[SerializeField]
	bool timerSet = false;
	[SerializeField]
	BoxCollider2D box = null;
	[SerializeField]
	GlobalController gc = null;

    void Start()
    {
		box = GetComponent<BoxCollider2D>();
		gc = FindObjectOfType<GlobalController>();
    }
    void Update()
    {
		if (gc.GetIsGameplay() && hasDisabled == false && timerSet == false)
		{
			currentTimer = defaultTimer;
			timerSet = true;
		}
		if (currentTimer > 0.0f)
		{
			currentTimer -= Time.deltaTime;
		}
		if (gc.GetIsGameplay() && currentTimer <= 0.0f && hasDisabled == false && timerSet == true)
		{
			box.GetComponent<BoxCollider2D>().enabled = false;
			hasDisabled = true;
		}
		if (gc.GetIsBuilding() && hasDisabled == true)
		{
			ResetThis();
		}
    }
	void ResetThis()
	{
		box.GetComponent<BoxCollider2D>().enabled = false;
		hasDisabled = false;
		timerSet = false;
		currentTimer = 0.0f;
	}
}
