using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	private GameObject player;
	private PlayerMovement playerMovementScript;

	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerMovementScript = player.GetComponent<PlayerMovement> ();
	}

	void LateUpdate()
	{
		//Iska pipane!

		if(playerMovementScript.facingRight)
			this.transform.position = Vector3.Lerp (this.transform.position + new Vector3(0.05f,0,0), player.transform.position, /*4 * Time.deltaTime*/ 0.04f) + new Vector3(0,0,-10f);
		else
			this.transform.position = Vector3.Lerp (this.transform.position + new Vector3(-0.05f,0,0), player.transform.position,  /*4 * Time.deltaTime*/ 0.04f) + new Vector3(0,0,-10f);
		
		if(Application.loadedLevelName == "Level5")
		{
			this.transform.position = new Vector3(this.transform.position.x,
			                                      this.transform.position.y + 0.06f,
			                                      this.transform.position.z);
		}
	}
}
