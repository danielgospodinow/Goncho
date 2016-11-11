using UnityEngine;
using System.Collections;

public class GameOverPortalLogic : MonoBehaviour 
{
	private GameObject player;
	public AudioClip gameCompleteSound;
	private GameObject canvas;
	private bool alreadyActivated = false;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		Physics2D.IgnoreCollision(this.gameObject.GetComponent<BoxCollider2D>(), player.GetComponent<BoxCollider2D>());
		canvas = GameObject.Find ("Canvas");
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			if(alreadyActivated == false)
				StartCoroutine(GameFinish());
		}
	}

	private IEnumerator GameFinish()
	{
		alreadyActivated = true;
		canvas.SetActive (false);
		player.GetComponent<PlayerMovement> ().OnStopMovingClick ();
		Destroy(GameObject.Find("MusicPlayer"));
		AudioSource.PlayClipAtPoint (gameCompleteSound, this.transform.position);
		yield return new WaitForSeconds(6f);
		Application.LoadLevel ("GameCompleteScene");
	}
}
