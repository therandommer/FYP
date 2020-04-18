using UnityEngine;

public class Ladder : MonoBehaviour
{
	void Awake()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1);
	}
}
