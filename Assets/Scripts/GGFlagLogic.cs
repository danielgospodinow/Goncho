using UnityEngine;
using System.Collections;

public class GGFlagLogic : MonoBehaviour {

	public AudioClip ggSound;

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			StartCoroutine("FlagLogic");
		}
	}

	private IEnumerator FlagLogic()
	{
		Destroy (GameObject.Find ("MusicPlayer"));
		Destroy (GameObject.Find ("Canvas"));
		GameObject.FindWithTag ("Player").GetComponent<PlayerMovement> ().OnStopMovingClick ();
		AudioSource.PlayClipAtPoint(ggSound, this.gameObject.transform.position);
		yield return new WaitForSeconds (1f);
		Application.LoadLevel("LevelCompleteScene");
	}
}
