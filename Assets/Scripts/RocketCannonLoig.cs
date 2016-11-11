using UnityEngine;
using System.Collections;

public class RocketCannonLoig : MonoBehaviour 
{
	private float shootTimer = 2f;
	public GameObject rocket;
	private GameObject rocketCannonDuo;
	public ParticleSystem rocketSpawnParticle;
	public AudioClip shootSound;

	void Start()
	{
		Instantiate (rocketSpawnParticle, this.gameObject.transform.position, Quaternion.Euler(-90,0,0));
		rocketCannonDuo = this.gameObject.transform.FindChild ("RocketDuo").gameObject;
		StartCoroutine (SelfDestroyer());
	}

	void Update()
	{
		if(shootTimer >= 2f)
			Shoot();
		else
			shootTimer+= 1*Time.deltaTime;
	}

	private void Shoot()
	{
		AudioSource.PlayClipAtPoint (shootSound,this.transform.position);
		GameObject currentRocket = Instantiate (rocket, rocketCannonDuo.transform.position, rocketCannonDuo.transform.rotation) as GameObject;
		currentRocket.GetComponent<RocketLogic> ().RocketCannon = this.gameObject;
		shootTimer = 0f;
	}

	private IEnumerator SelfDestroyer()
	{
		yield return new WaitForSeconds (10f);
		Instantiate (rocketSpawnParticle, this.gameObject.transform.position, Quaternion.Euler(-90,0,0));
		Destroy (this.gameObject);
	}

//	void OnTriggerEnter2D(Collider2D col)
//	{
//		if(col.gameObject.tag == "Enemy")
//		{
//			BoxCollider2D thisCol = this.gameObject.GetComponent<BoxCollider2D>();
//			BoxCollider2D enemyBoxCol = col.gameObject.GetComponent<BoxCollider2D>();
//			CircleCollider2D enemyCirlCol = col.gameObject.GetComponent<CircleCollider2D>();
//
//			if(enemyBoxCol != null)
//				Physics2D.IgnoreCollision(thisCol, enemyBoxCol);
//			if(enemyCirlCol != null)
//				Physics2D.IgnoreCollision(thisCol, enemyCirlCol);
//		}
//	}
}
