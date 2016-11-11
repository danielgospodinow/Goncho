using UnityEngine;
using System.Collections;

public class SelfDestructionScript : MonoBehaviour 
{
	void Start () 
	{
		if(this.gameObject.name == "SlimeMonsterAttackParticle(Clone)")
			Destroy(this.gameObject, 7f);
		else
			Destroy(this.gameObject, 3f);
	}

	void Update()
	{
		if(HealthBarLogic.alreadyDead)
		{
			Destroy(this.gameObject);
		}
	}
}
