using UnityEngine;
using System.Collections;

public class YellowMonsterParticleLogic : MonoBehaviour 
{
	private GameObject objectToFollow;

	public GameObject ObjectToFollow
	{
		set { this.objectToFollow = value;}
	}

	void Update()
	{
		if(objectToFollow == null)
			return;

		this.gameObject.transform.position = new Vector3 (
			objectToFollow.transform.position.x,
			objectToFollow.transform.position.y,
			0);
	}
}
