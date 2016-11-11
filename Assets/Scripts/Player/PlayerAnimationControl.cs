using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerAnimationControl : MonoBehaviour 
{
	private PlayerMovement playerMovementScript;
	private Animator playerAnim;
	[HideInInspector]
	public bool isKneeingButtonClicked = false;
	[HideInInspector]
	public bool playerShooting = false;
	private Button shootButton;

	void Start()
	{
		playerMovementScript = GetComponent<PlayerMovement> ();
		playerAnim = GetComponent<Animator> ();

		if (Application.loadedLevelName == "MainMenu")
			return;

		shootButton = GameObject.Find("Canvas").transform.FindChild("ButtonShoot").GetComponent<Button>();

		StartCoroutine (KneePreventer(0.5f));
	}

	public IEnumerator KneePreventer(float seconds)
	{
		GameObject.Find ("Canvas").transform.FindChild ("ButtonDown").GetComponent<Button>().enabled = false;
		yield return new WaitForSeconds (seconds);
		GameObject.Find ("Canvas").transform.FindChild ("ButtonDown").GetComponent<Button>().enabled = true;
	}

	void Update()
	{
		if(HealthBarLogic.alreadyDead)
			return;

		playerAnim.SetFloat("animSpeed", Mathf.Abs(playerMovementScript.horMove));
		playerAnim.SetBool ("animJumping", playerMovementScript.isJumping);

		if(isKneeingButtonClicked && playerMovementScript.isKneeing == false)
		{
			playerAnim.SetBool("animKneeing", true);
			playerMovementScript.isKneeing = true;
		}
		if(isKneeingButtonClicked == false && playerMovementScript.isKneeing == true)
		{
			playerAnim.SetBool("animKneeing", false);
			playerMovementScript.isKneeing = false;
		}
	}

	public void OnButtonDownClick()
	{
		if(HealthBarLogic.alreadyDead)
			return;

		if(playerAnim.GetBool("animJumping") == false)
			isKneeingButtonClicked = true;
	}

	public void OnUpButtonClick()
	{
		isKneeingButtonClicked = false;
	}

	public void OnShootButtonClick()
	{
		if(HealthBarLogic.alreadyDead)
			return;

		playerShooting = !playerShooting;
		if(playerShooting)
			GameObject.Find ("ButtonShoot").GetComponent<Image> ().color = new Color32(255,0,0,244);
		else
		{
			GameObject.Find ("ButtonShoot").GetComponent<Image> ().color = new Color32(255,255,255,224);
			StartCoroutine("DisableShooting");
		}
		playerAnim.SetBool ("animShooting", playerShooting);
	}

	private IEnumerator DisableShooting()
	{;
		shootButton.interactable = false;
		yield return new WaitForSeconds(0.7f);
		shootButton.interactable = true;
	}
}
