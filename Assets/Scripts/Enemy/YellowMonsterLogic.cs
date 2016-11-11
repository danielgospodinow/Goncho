using UnityEngine;
using System.Collections;

public class YellowMonsterLogic : Monster 
{
	public ParticleSystem yellowMonsterAttackParticle;
	public ParticleSystem yellowMonsterHurtParticle;

	protected override void Start ()
	{
		base.Start ();
		inRangeToAttackValue = 2.6f;
		isDazed = false;
		armor = 5;
		monsterHurtParticle = this.yellowMonsterHurtParticle;
	}

	protected override void Update ()
	{
		base.Update ();
	}

	public override void DamageMonster (int damage, bool bigRocket)
	{
		base.DamageMonster (damage, bigRocket);
		if(bigRocket)
			StartCoroutine ("MonsterSelfDazer");
	}

	protected override void DamagePlayer (int damage)
	{
		if(canDamage && shouldDamage && HealthBarLogic.alreadyDead == false)
			StartCoroutine("YellowMonsterAttack");
	}

	private IEnumerator YellowMonsterAttack()
	{
		canDamage = false;
		if(isFacingRight == false)
			Instantiate (yellowMonsterAttackParticle, this.gameObject.transform.position + new Vector3(0,0,-0.6f), Quaternion.Euler (-12, -90, 0));
		else
			Instantiate (yellowMonsterAttackParticle, this.gameObject.transform.position + new Vector3(0,0,-0.6f), Quaternion.Euler (-12, 90, 0));
		playerHealthBar.GetComponent<HealthBarLogic> ().DamagePlayer (10);
		yield return new WaitForSeconds(0.5f);
		canDamage = true;
	}

	public override void CriticalDamageMonster (int damage, bool bigRocket)
	{
		base.CriticalDamageMonster (damage, bigRocket);
		if(bigRocket)
			StartCoroutine("MonsterSelfDazer");
	}
}
