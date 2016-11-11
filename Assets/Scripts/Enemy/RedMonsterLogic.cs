using UnityEngine;
using System.Collections;

public class RedMonsterLogic : Monster {

	public ParticleSystem redMonsterAttackParticle;
	public ParticleSystem redMonsterHurtParticle;

	protected override void Start ()
	{
		base.Start ();

		inRangeToAttackValue = 0.8f;
		armor = 15;
		monsterHurtParticle = this.redMonsterHurtParticle;
	}

	protected override void Update ()
	{
		base.Update ();
	}

	protected override void DamagePlayer (int damage)
	{
		if(canDamage && shouldDamage && HealthBarLogic.alreadyDead == false)
			StartCoroutine ("RedMonsterAttacker");
	}

	public override void DamageMonster (int damage, bool bigRocket)
	{
		base.DamageMonster (damage, bigRocket);
		if(bigRocket)
			StartCoroutine ("MonsterSelfDazer");
	}

	private IEnumerator RedMonsterAttacker()
	{
		canDamage = false;
		if(player.GetComponent<PlayerAnimationControl>().isKneeingButtonClicked)
			player.GetComponent<PlayerAnimationControl>().OnUpButtonClick();
		yield return new WaitForSeconds (0.1f);
		if(isFacingRight == false)
			Instantiate (redMonsterAttackParticle, this.gameObject.transform.position + new Vector3(0,0,-0.03f), Quaternion.Euler(0,-90,0));
		else
			Instantiate (redMonsterAttackParticle, this.gameObject.transform.position + new Vector3(0,0,-0.03f), Quaternion.Euler(0,90,0));
		player.GetComponent<Rigidbody2D> ().AddForce (new Vector2(player.transform.position.x - this.gameObject.transform.position.x, player.transform.position.y - this.gameObject.transform.position.y) * 300f);
		playerHealthBar.GetComponent<HealthBarLogic> ().DamagePlayer (25);
		yield return new WaitForSeconds(2);
		canDamage = true;
	}

	public override void CriticalDamageMonster (int damage, bool bigRocket)
	{
		base.CriticalDamageMonster (damage, bigRocket);
		if(bigRocket)
			StartCoroutine("MonsterSelfDazer");
	}
}
