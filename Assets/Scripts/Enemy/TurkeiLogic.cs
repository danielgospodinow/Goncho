using UnityEngine;
using System.Collections;

public class TurkeiLogic : MonoBehaviour 
{
	private HealthBarLogic healthBar;
	private Animator enemyAnim;
	private bool isRunning;
	[SerializeField]
	private bool isFacingRight;
	private SpriteRenderer enemySprite;
	private float currentXPos;
	private float wantedXPos;
	private float runDelay = 3f;
	private float currentTime = 0f;
	private bool timeToRun = true;
	private bool readyToDamage = true;
	private GameObject player;

	void Awake()
	{
		healthBar = GameObject.Find ("HealthBar").GetComponent<HealthBarLogic>();
		enemyAnim = GetComponent<Animator> ();
		isRunning = false;
		if(enemyAnim != null)
			enemyAnim.SetBool ("IsRunning", isRunning);
		enemySprite = GetComponent<SpriteRenderer> ();
		isFacingRight = false;
		currentXPos = this.gameObject.transform.position.x;
		wantedXPos = currentXPos + 2.5f;
		EnemyFlip ();

		player = GameObject.FindGameObjectWithTag ("Player");
		
		if(this.GetComponent<BoxCollider2D>() != null)
		{
			Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), player.GetComponent<BoxCollider2D>());
			Physics2D.IgnoreCollision(this.GetComponent<BoxCollider2D>(), player.GetComponent<CircleCollider2D>());
		}
	}

	void FixedUpdate()
	{
		if(enemyAnim != null)
			enemyAnim.SetBool ("IsRunning", isRunning);
		currentXPos = this.gameObject.transform.position.x;

		if(timeToRun)
		{
			if(isFacingRight)
			{
				if(currentXPos < wantedXPos)
				{
					this.transform.position += new Vector3 (0.7f, 0, 0) * Time.deltaTime;
					isRunning = true;
				}
				else
				{
					isRunning = false;
					currentXPos = wantedXPos;
					wantedXPos = wantedXPos - 2.5f;
					EnemyFlip();
					timeToRun = false;
				}
			}
			else if(isFacingRight == false)
			{
				if(currentXPos > wantedXPos)
				{
					this.transform.position += new Vector3 (-0.7f, 0, 0) * Time.deltaTime;
					isRunning = true;
				}
				else
				{
					isRunning = false;
					currentXPos = wantedXPos;
					wantedXPos = wantedXPos + 2.5f;
					EnemyFlip();
					timeToRun = false;
				}
			}
		}
		else
		{
			currentTime += 1 * Time.deltaTime;
			if(currentTime > runDelay)
			{
				timeToRun = true;
				currentTime = 0f;
			}
		}
	}

	void EnemyFlip()
	{
		Vector3 theScale = enemySprite.transform.localScale;
		theScale.x *= -1;
		enemySprite.transform.localScale = theScale;
		isFacingRight = !isFacingRight;
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if(readyToDamage == false)
			return;

		if (col.gameObject.tag == "Player") 
		{
			if(this.gameObject.name == "Turkey")
			{
				healthBar.DamagePlayer(10);
				Destroy(gameObject);
			}
			else if(col.gameObject.tag == "Player" && (this.gameObject.name == "SpikeMonster01" || this.gameObject.name == "SpikeMonster02") && HealthBarLogic.alreadyDead == false)
			{
				healthBar.DamagePlayer(30);
				col.gameObject.GetComponent<Rigidbody2D>().AddForce(col.gameObject.transform.position + this.gameObject.transform.position + new Vector3(0,4f,0) * 70f);
				col.gameObject.GetComponent<PlayerMovement>().isJumping = true;
				StartCoroutine("DamageCooldown");
			}
		}
	}

	private IEnumerator DamageCooldown()
	{
		readyToDamage = false;
		yield return new WaitForSeconds(1f);
		readyToDamage = true;
	}
}
