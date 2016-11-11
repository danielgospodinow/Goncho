using UnityEngine;
using System.Collections;

public class ShadowMonsterLogic : Monster 
{
	public ParticleSystem shadowMonsterHurtParticle;
	public ParticleSystem shadowMonsterSpitParticle;
	public ParticleSystem shadowMonsterBlowParticle;
	public ParticleSystem shadowMonsterKillParticle;
	public ParticleSystem shadowMonsterSlowParticle;
	public GameObject gameOverPortal;
	private bool canBlow = true;
	private bool canSpit = true;

	protected override void Start ()
	{
		base.Start ();
		
		inRangeValue = 10f;
		inRangeToAttackValue = 4f;
		armor = 25;
		health = 100;
		base.monsterHurtParticle = this.shadowMonsterHurtParticle;
		shouldDamage = true;
	}
	
	protected override void Update ()
	{
		base.Update ();

		if(base.inRange)
			DamagePlayer(0);
	}

	protected override void KillMonster ()
	{
		base.KillMonster ();
		Instantiate (shadowMonsterKillParticle, this.gameObject.transform.position, Quaternion.Euler(0, 0, 0));
		Instantiate (gameOverPortal, this.gameObject.transform.position, Quaternion.Euler (0, 0, 0));
		player.GetComponent<PlayerMovement> ().speed = 12f;
	}

	protected override void DamagePlayer (int damage)
	{
		if(canDamage && HealthBarLogic.alreadyDead == false)
		{
			if(base.distanceToPlayer <= 4f && canSpit)
			{
				base.playerHealthBar.GetComponent<HealthBarLogic>().DamagePlayer(5);
				StartCoroutine(ShadowSpitAttack());
			}
			else if(base.distanceToPlayer > 4f && canBlow)
			{
				if(base.distanceToPlayer <= 7.8f)
				{
					base.playerHealthBar.GetComponent<HealthBarLogic>().DamagePlayer(10);
					StartCoroutine(ShadowSlow());
				}

				StartCoroutine(ShadowBlowAttack());
			}
		}
	}

	private IEnumerator ShadowSlow()
	{
		Instantiate (shadowMonsterSlowParticle);
		player.GetComponent<PlayerMovement> ().speed = 8.5f;
		yield return new WaitForSeconds(3f);
		player.GetComponent<PlayerMovement> ().speed = 12f;
	}

	private IEnumerator ShadowBlowAttack()
	{
		canBlow = false;
		Instantiate(shadowMonsterBlowParticle, this.transform.position + new Vector3(+0.7f,-2.3f,0), Quaternion.Euler(-90,0,0));
		yield return new WaitForSeconds(3f);
		canBlow = true;
	}

	private IEnumerator ShadowSpitAttack()
	{
		canSpit = false;
		if(this.gameObject.transform.position.x > player.transform.position.x)
			Instantiate(this.shadowMonsterSpitParticle, this.transform.position + new Vector3(0,-1f,-0.2f), Quaternion.Euler(0,-90,0));
		else
			Instantiate(this.shadowMonsterSpitParticle, this.transform.position + new Vector3(0,-1f,-0.2f), Quaternion.Euler(0,90,0));
		yield return new WaitForSeconds(0.5f);
		canSpit = true;
	}
}
