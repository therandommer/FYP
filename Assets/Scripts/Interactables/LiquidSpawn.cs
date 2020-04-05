using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 5);
    }
}
