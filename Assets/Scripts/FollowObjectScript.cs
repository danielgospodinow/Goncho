using UnityEngine;
using System.Collections;

public class FollowObjectScript : MonoBehaviour 
{
	private GameObject objectToFollow;

	void Awake () 
	{
		if(this.gameObject.name == "ShootParticle(Clone)")
			objectToFollow = GameObject.Find ("WeaponDuo");
		else if(this.gameObject.name == "PlayerHealParticle(Clone)" || this.gameObject.name == "ShadowSlowParticle(Clone)")
			objectToFollow = GameObject.Find ("Player");
	}

	void Update () 
	{
		if(objectToFollow == null)
			return;

		if(this.gameObject.name == "ShadowSlowParticle(Clone)")
			this.gameObject.transform.position = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y -0.3f, +1f);
		else
			this.gameObject.transform.position = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y, objectToFollow.transform.position.z - 1.5f);
	}
}
