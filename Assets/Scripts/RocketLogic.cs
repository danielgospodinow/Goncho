using UnityEngine;
using System.Collections;

public class RocketLogic : MonoBehaviour 
{
	private float bulletSpeed = 8f;
	private int directionValue;
	public AudioClip gunShot;
	public ParticleSystem rocketParticle;
	private float currentSide;
	private GameObject rocketCannon;
	private bool alreadyHit = false;

	public GameObject RocketCannon
	{
		set{this.rocketCannon = value;}
	}

	void Start()
	{
		//AudioSource.PlayClipAtPoint (gunShot, this.gameObject.transform.position);
		Destroy (this.gameObject, 6f);
	}

	void Update()
	{
		this.gameObject.transform.Translate (Vector3.left *Time.deltaTime * bulletSpeed);
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.name == Enemies.SlimeMonster)
		{
			DamageHander(col.gameObject.GetComponent<SlimeMonsterLogic>());
		}
		
		else if(col.gameObject.name == Enemies.BlueMonster)
		{
			DamageHander(col.gameObject.GetComponent<BlueMonsterLogic>());
		}
		
		else if(col.gameObject.name == Enemies.RedMonster)
		{
			DamageHander(col.gameObject.GetComponent<RedMonsterLogic>());
		}
		
		else if(col.gameObject.name == Enemies.YellowMonster)
		{
			DamageHander(col.gameObject.GetComponent<YellowMonsterLogic>());
		}

		else if(col.gameObject.name == Enemies.ShadowMonster)
		{
			DamageShadowMonster(col.gameObject.GetComponent<ShadowMonsterLogic>());
		}

		else if(col.gameObject.tag == "Enemy")
		{
			Destroy(col.gameObject);
			DamageHander(null);
		} 
		
		else if(col.gameObject.tag == "GroundElement" && (col.gameObject.name != "RocketCannon(Clone)" && col.gameObject.name != "RocketCannon"))
		{
			Destroy(this.gameObject);
		}
	}

	private void DamageShadowMonster(Monster shadowMonsterScript)
	{
		if(alreadyHit)
			return;

		alreadyHit = true;
		shadowMonsterScript.DamageMonster (Random.Range(32,37), false);
		Destroy (this.gameObject);
	}
	
	private void DamageHander(Monster monsterScript)
	{
		if(monsterScript == null)
		{
			Destroy(this.gameObject);
			return;
		}

		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		for(int i=0; i < enemies.Length; i++)
		{
			float distanceToEnemies = Vector2.Distance(enemies[i].transform.position, this.gameObject.transform.position);
			
			if(distanceToEnemies < 3f)
			{
				if(enemies[i].GetComponent<Monster>() != null)
				{
					enemies[i].GetComponent<Monster>().DamageMonster(Random.Range(85,95), true);
				}
				else
					enemies[i].SetActive(false);
			}
		}

		Destroy(this.gameObject);
	}
}
