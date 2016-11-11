using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonPointerUpDown : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
	private bool rightButton;
	private PlayerMovement playerMovementScript;

	void Awake()
	{
		playerMovementScript = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();

		if(this.gameObject.name == "ButtonRight")
			rightButton = true;
		else
			rightButton = false; // => ButtonLeft
	}

	public void OnPointerDown(PointerEventData data)
	{
		if(rightButton)
			playerMovementScript.OnRightButtonClick();
		else
			playerMovementScript.OnLeftButtonClick();
	}

	public void OnPointerUp(PointerEventData data)
	{
		playerMovementScript.OnStopMovingClick ();
	}
}
