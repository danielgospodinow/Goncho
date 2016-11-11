using UnityEngine;
using System.Collections;

public class SpikesLogic : MonoBehaviour 
{
	public bool hardSpikes = false;
	private bool canDamage = true;

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player" && HealthBarLogic.alreadyDead == false && hardSpikes == false && canDamage)
		{
			GameObject.Find("HealthBar").GetComponent<HealthBarLogic>().DamagePlayer(30);
			col.gameObject.GetComponent<Rigidbody2D>().AddForce(col.gameObject.transform.position + this.gameObject.transform.position + new Vector3(0,2f,0) * 70f);
			col.gameObject.GetComponent<PlayerMovement>().isJumping = true;
			StartCoroutine("DamageCooldown");
		}

		else if(col.gameObject.tag == "Player" && HealthBarLogic.alreadyDead == false && hardSpikes)
		{
			GameObject.Find("HealthBar").GetComponent<HealthBarLogic>().DamagePlayer(200);
		}
	}

	private IEnumerator DamageCooldown()
	{
		canDamage = false;
		yield return new WaitForSeconds(1f);
		canDamage = true;
	}
}
