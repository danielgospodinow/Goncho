using UnityEngine;
using System.Collections;

public class BulletsLogic : MonoBehaviour 
{
	private float bulletSpeed = 8f;
	private PlayerMovement playerMovementScript;
	private int directionValue;
	public AudioClip gunShot;
	public ParticleSystem playerHurtParticle;
//	public ParticleSystem slimeMonsterHurtParticle;
//	public ParticleSystem blueMonsterDamageParticle;
//	public ParticleSystem redMonsterDamageParticle;
//	public ParticleSystem yellowMonsterDamageParticle;
	private bool isBigRocket;
	private int criticalChanceRoll;

	void Awake()
	{
		AudioSource.PlayClipAtPoint (gunShot, this.gameObject.transform.position);
		playerMovementScript = GameObject.Find ("Player").GetComponent<PlayerMovement> ();
		isBigRocket = playerMovementScript.isKneeing;

		if (playerMovementScript.facingRight)
			directionValue = 1;
		else
			directionValue = -1;

		Destroy (this.gameObject, 1f);
	}

	void Start()
	{
		criticalChanceRoll = Random.Range (0, 4);
	}

	void Update()
	{
		this.gameObject.transform.position += new Vector3 (directionValue, 0, 0) * Time.deltaTime * bulletSpeed;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.name == Enemies.SlimeMonster)
		{
			DamageHander(//slimeMonsterHurtParticle, 
			             col.gameObject.GetComponent<SlimeMonsterLogic>(), 30, 50);
		}

		else if(col.gameObject.name == Enemies.BlueMonster)
		{
			DamageHander(//blueMonsterDamageParticle, 
			             col.gameObject.GetComponent<BlueMonsterLogic>(), 30, 50);
		}

		else if(col.gameObject.name == Enemies.RedMonster)
		{
			DamageHander(//redMonsterDamageParticle, 
			             col.gameObject.GetComponent<RedMonsterLogic>(), 30, 50);
		}

		else if(col.gameObject.name == Enemies.YellowMonster)
		{
			DamageHander(//yellowMonsterDamageParticle, 
			             col.gameObject.GetComponent<YellowMonsterLogic>(), 30, 50);
		}

		else if(col.gameObject.name == Enemies.ShadowMonster)
		{
			DamageHander(//blueMonsterDamageParticle, 
			             col.gameObject.GetComponent<ShadowMonsterLogic>(), 30, 34);
		}

		else if(col.gameObject.name == Enemies.CarRobor)
		{
			col.gameObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2(this.gameObject.transform.position.x - GameObject.Find("Player").transform.position.x, this.gameObject.transform.position.y - GameObject.Find("Player").transform.position.y) * 900f);
			Destroy(this.gameObject);
		}
		else if(col.gameObject.tag == "Enemy")
		{
			Destroy(col.gameObject);
			DamageHander(null, 0, 0);
		} 

		else if(col.gameObject.tag == "GroundElement")
		{
			Destroy(this.gameObject);
		}
	}

	private void DamageHander(/*ParticleSystem particleSystem, */Monster monsterScript, int smallRocketHit, int bigRocketHit)
	{
		if(monsterScript == null)
		{
			Instantiate(playerHurtParticle, this.transform.position, this.transform.rotation);
			Destroy(this.gameObject);
			return;
		}

		bool criticalHit = (criticalChanceRoll == 0); // Случайност със стойност 1/
		if(criticalHit)
		{
			//Instantiate(particleSystem, this.transform.position, this.transform.rotation);
			if(isBigRocket == false)
				monsterScript.CriticalDamageMonster(RandomDamageAround(smallRocketHit), false);
			else if (isBigRocket)
				monsterScript.CriticalDamageMonster(RandomDamageAround(bigRocketHit), true);
			Destroy(this.gameObject);

			return;
		}

		//Instantiate(particleSystem, this.transform.position, this.transform.rotation);
		if(isBigRocket == false)
			monsterScript.DamageMonster(RandomDamageAround(smallRocketHit), false);
		else if (isBigRocket)
			monsterScript.DamageMonster(RandomDamageAround(bigRocketHit), true);
		Destroy(this.gameObject);
	}

	private int RandomDamageAround(int aroundDamage)
	{ 
		return Random.Range(aroundDamage - 2, aroundDamage + 2);
	}
}
