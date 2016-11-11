using UnityEngine;
using System.Collections;

public class SlimeMonsterParticleLogic : MonoBehaviour {

	private float attackTime = 1.5f;
	private bool inRange;

	void Update()
	{
		if(attackTime > 1.5f && inRange && HealthBarLogic.alreadyDead == false)
		{
			attackTime = 0f;
			GameObject.Find("HealthBar").GetComponent<HealthBarLogic>().DamagePlayer(2);
		}
		else
		{
			attackTime += 1 * Time.deltaTime;
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
			inRange = true;
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
			inRange = false;
	}
}
