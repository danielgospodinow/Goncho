using UnityEngine;
using System.Collections;

public class BoundsLogic : MonoBehaviour 
{
	HealthBarLogic playerHealthBarLogic;

	void Start()
	{
		playerHealthBarLogic = GameObject.Find ("HealthBar").GetComponent<HealthBarLogic> ();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" && playerHealthBarLogic != null)
		{
			playerHealthBarLogic.DamagePlayer(playerHealthBarLogic.Health);
		}
		else if(col.gameObject.name == "CarRobot")
		{
			Destroy(col.gameObject);
		}
	}
}
