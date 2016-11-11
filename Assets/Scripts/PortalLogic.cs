using UnityEngine;
using System.Collections;

public class PortalLogic : MonoBehaviour 
{
	public Vector3 TeleportTo;
	public AudioClip teleportClip;
	public ParticleSystem teleportParticle;

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player") 
		{
			col.gameObject.transform.position = TeleportTo + new Vector3(0,0,-0.1f);
			col.gameObject.GetComponent<PlayerMovement>().OnStopMovingClick();
			StartCoroutine("TeleportParticleEnabler", col.gameObject.transform.position);
			AudioSource.PlayClipAtPoint(teleportClip, col.gameObject.transform.position);
		}
	}

	private IEnumerator TeleportParticleEnabler(Vector3 pos)
	{
		yield return new WaitForSeconds (0.1f);
		Instantiate (teleportParticle, pos, Quaternion.Euler (-90, 0, 0));
	}
}
