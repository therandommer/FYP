using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    void Awake()
    {
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
