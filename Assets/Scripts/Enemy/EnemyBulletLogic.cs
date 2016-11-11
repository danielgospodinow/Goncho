using UnityEngine;
using System.Collections;

public class EnemyBulletLogic : MonoBehaviour 
{
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			GameObject.Find("HealthBar").GetComponent<HealthBarLogic>().DamagePlayer(35);
			Destroy(this.gameObject);
			Destroy(this);
		}
	}
}
