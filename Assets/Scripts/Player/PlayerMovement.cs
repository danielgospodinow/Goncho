using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour 
{
	[HideInInspector]
	public bool facingRight;
	[HideInInspector]
	public float verMove;
	[HideInInspector]
	public float horMove;
	[HideInInspector]
	public float speed;
	[HideInInspector]
	public Rigidbody2D playerRigid;
	private SpriteRenderer spriteRenderer;
	[HideInInspector]
	public bool isJumping;
	private float jumpForce;
	[HideInInspector]
	public bool isKneeing;
	private PlayerAnimationControl playerAnimControlScript;

	void Start()
	{
		facingRight = true;
		speed = 12f;
		jumpForce = 500f;
		playerRigid = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		isJumping = false;
		isKneeing = false;
		playerAnimControlScript = this.gameObject.GetComponent<PlayerAnimationControl> ();
	}

	void FixedUpdate()
	{
		if(HealthBarLogic.alreadyDead)
			return;

		playerRigid.velocity = Vector3.ClampMagnitude(playerRigid.velocity, 12f); 

#if UNITY_EDITOR
		if(Input.GetKey(KeyCode.D))
			OnRightButtonClick();
		else if(Input.GetKey(KeyCode.A))
			OnLeftButtonClick();
		else if(Input.GetKey(KeyCode.S))
			playerAnimControlScript.OnButtonDownClick();
		else
			OnStopMovingClick();

		if(Input.GetKeyDown(KeyCode.R))
	    {
			PlayerScore.money += 100;
		}

		if(Input.GetKey(KeyCode.Space))
			OnJumpButtonClick();
#endif

		Moving (horMove);
	}

	void Moving(float hMove)
	{
		if(hMove != 0 && isKneeing)
			playerAnimControlScript.OnUpButtonClick();

		if(hMove > 0)
		{
			playerRigid.AddForce(Vector2.right * hMove * speed);
			if(facingRight == false && this.playerAnimControlScript.playerShooting == false)
				FlipPlayer();
		}
		else if(hMove < 0)
		{
			playerRigid.AddForce(Vector2.right * hMove * speed);
			if(facingRight && this.playerAnimControlScript.playerShooting == false)
				FlipPlayer();
		}
	}

	void Jumping(float vMove)
	{
		if(isJumping == false && HealthBarLogic.alreadyDead == false)
		{
			playerRigid.AddForce (Vector2.up * jumpForce * vMove);
			isJumping = true;
		}
	}

	void FlipPlayer()
	{
		Vector3 theScale = spriteRenderer.transform.localScale;
		theScale.x *= -1;
		spriteRenderer.transform.localScale = theScale;
		facingRight = !facingRight;
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if(col.gameObject.tag == "GroundElement" && isJumping)
			isJumping = false;
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject.tag == "GroundElement" && isJumping == false)
			isJumping = true;
	}

	public void OnRightButtonClick()
	{
		horMove = 1f;
	}

	public void OnLeftButtonClick()
	{
		horMove = -1f;
	}

	public void OnStopMovingClick()
	{
		horMove = 0f;
	}

	public void OnJumpButtonClick()
	{
		if(playerAnimControlScript.isKneeingButtonClicked)
		{
			StartCoroutine(FromKneeToJump());
			return;
		}
		if(playerAnimControlScript.playerShooting)
			playerAnimControlScript.OnShootButtonClick();

		Jumping (1);
	}

	private IEnumerator FromKneeToJump()
	{
		playerAnimControlScript.OnUpButtonClick();
		yield return new WaitForSeconds (0.05f);
		this.OnJumpButtonClick ();
	}
}
