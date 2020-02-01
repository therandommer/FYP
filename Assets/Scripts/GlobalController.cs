﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
	#region States
	[Header("States")]
	[SerializeField]
	bool isPaused = false;
	[SerializeField]
	bool isBuilding = true; //when true building UI enabled, when false playing level.
	[SerializeField]
	bool isSaving = false;
	#endregion

	void Start()
    {
        
    }

    void Update()
    {
        
    }
}
