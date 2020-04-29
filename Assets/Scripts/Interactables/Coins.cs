using UnityEngine;

public class Coins : MonoBehaviour
{
	BuildSettings build = null;
	[SerializeField]
	SpriteRenderer sprite = null;
	[SerializeField]
	BoxCollider2D col = null;
	GlobalController gc = null;
	PlatformingManager platform = null;

	[SerializeField]
	bool hasCollected = false;
	[SerializeField]
	private int coinValue = 1;
	[SerializeField]
	private bool isHealth = false;
	[SerializeField]
	private int healthValue = 1;
	[SerializeField]
	private int scoreValue = 100;

	// Start is called before the first frame update
	void Start()
	{
		build = FindObjectOfType<BuildSettings>();
		platform = FindObjectOfType<PlatformingManager>();
		gc = FindObjectOfType<GlobalController>();
		col = GetComponent<BoxCollider2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			hasCollected = true;
			if (!isHealth)
			{
				platform.IncrementCoins(coinValue);
			}
			if(isHealth)
			{
				collision.gameObject.SendMessage("Heal", healthValue);
			}
			platform.IncrementScore(scoreValue);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (!gc.GetIsBuilding() && hasCollected) //disables object when collected
		{
			sprite.enabled = false;
			col.enabled = false;
		}
		else if (gc.GetIsBuilding()) //re-enables in the build screen
		{
			hasCollected = false;
			sprite.enabled = true;
			col.enabled = true;
		}
	}
}
