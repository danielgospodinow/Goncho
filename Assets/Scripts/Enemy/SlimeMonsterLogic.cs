using UnityEngine;
using System.Collections;

public class SlimeMonsterLogic : Monster 
{
	public ParticleSystem slimeMonsterAttackParticle;
	public ParticleSystem slimeMonsterHurtParticle;

	protected override void Start ()
	{
		base.Start ();

		inRangeToAttackValue = 0.9f;
		inRangeValue = 6f;
		animatorAttackVariable = "slimeAttack";
		animatorSpeedVariable = "slimeSpeed";
		armor = 20;
		sspeed = 0.4f;
		monsterHurtParticle = this.slimeMonsterHurtParticle;
	}

	protected override void Update()
	{
		base.Update ();

		if(inRange)
		{
			if(shouldDamage)
				return;

			SetSpeed(2);
			//this.monsterAnimator.SetBool(animatorAttackVariable, false);
		}
		else
		{
			SetSpeed(0);
			//this.monsterAnimator.SetBool(animatorAttackVariable, true);
		}
	}

	public override void DamageMonster (int damage, bool bigRocket)
	{
		base.DamageMonster (damage, bigRocket);
	}

	protected override void DamagePlayer(int damage)
	{
		if(this.gameObject.activeInHierarchy == false)
			return;

		monsterAnimator.SetBool(animatorAttackVariable, true);
		if(this.gameObject.activeInHierarchy)
			StartCoroutine("SlimeDamager");
	}

	private IEnumerator SlimeDamager()
	{
		if(shouldDamage && canDamage)
		{
			canDamage = false;
			playerHealthBar.GetComponent<HealthBarLogic>().DamagePlayer(10);
			Instantiate(slimeMonsterAttackParticle, this.gameObject.transform.position - new Vector3(0,0.5f,0.6f), Quaternion.Euler(-90,0,0));
			yield return new WaitForSeconds (1f);
			canDamage = true;
		}
	}
}
