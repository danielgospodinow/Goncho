using UnityEngine;
using System.Collections;

public class BlueMonsterLogic : Monster
{
	public GameObject spikedBall;
	public ParticleSystem blueMonsterHurtParticle;

	protected override void Start ()
	{
		base.Start ();

		inRangeValue = 5f;
		inRangeToAttackValue = 3f;
		armor = 0;
		monsterHurtParticle = this.blueMonsterHurtParticle;
	}

	protected override void Update ()
	{
		base.Update ();
	}

	protected override void DamagePlayer (int damage)
	{
		if(this.gameObject.GetComponent<Animator>().GetBool("hit"))
			return;

		if(canDamage && shouldDamage && HealthBarLogic.alreadyDead == false)
			StartCoroutine ("BlueMonsterShooter");
	}

	private IEnumerator BlueMonsterShooter()
	{
		canDamage = false;
		yield return new WaitForSeconds (0.7f);
		GameObject spikedBallO = Instantiate (spikedBall, this.gameObject.transform.position + new Vector3(0,0,-0.03f), Quaternion.Euler(270,0,0)) as GameObject;
		if(isFacingRight == false)
			spikedBallO.GetComponent<Rigidbody2D>().AddForce(new Vector2(-450*0.7f,100*1.2f));
		else
			spikedBallO.GetComponent<Rigidbody2D>().AddForce(new Vector2(450*0.7f,100*1.2f));
		yield return new WaitForSeconds (3f);
		canDamage = true;
	}

	public override void DamageMonster (int damage, bool bigRocket)
	{
		base.DamageMonster (damage, bigRocket);
		if(bigRocket)
		{
			StopAllCoroutines();
			StartCoroutine("MonsterSelfDazer");
		}
	}

	public override void CriticalDamageMonster (int damage, bool bigRocket)
	{
		base.CriticalDamageMonster (damage, bigRocket);
		if(bigRocket)
		{
			StopAllCoroutines();
			StartCoroutine("MonsterSelfDazer");
		}
	}
}
