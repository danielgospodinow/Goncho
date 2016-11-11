using UnityEngine;
using System.Collections;

public abstract class Monster : MonoBehaviour 
{
	protected int health = 100;
	protected bool isFacingRight;
	protected float distanceToPlayer;
	protected bool shouldDamage = false;
	protected bool canDamage = true;
	protected Animator monsterAnimator;
	protected GameObject player;
	protected GameObject playerHealthBar;
	protected Vector2 playerLocation;
	protected bool inRange = false;
	protected float speed = 2f;
	protected float inRangeValue = 5f;
	protected float inRangeToAttackValue;
	protected string animatorAttackVariable;
	protected string animatorSpeedVariable;
	protected bool isDazed = false;
	protected int score = 5;
	protected int armor = 0;
	private float timer;
	private GameObject monsterHealthBar;
	protected float sspeed = 0.7f;
	protected ParticleSystem monsterHurtParticle = null;

	protected virtual void Start()
	{
		playerHealthBar = GameObject.Find("HealthBar");
		monsterAnimator = this.gameObject.GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		monsterHealthBar = this.gameObject.GetComponentsInChildren<SpriteRenderer> () [2].gameObject;

		SetAllCollisionIngor ();
	}

	protected virtual void Update()
	{
		if(player == null)
			return;
		
		if (health <= 0) 
		{
			KillMonster();
		}

		playerLocation = player.transform.position;
		distanceToPlayer = Vector3.Distance (this.gameObject.transform.position, player.gameObject.transform.position);

		if(this.gameObject.name == "ShadowMonster")
			inRange = distanceToPlayer < inRangeValue;
		else
			inRange = distanceToPlayer < inRangeValue && 
				player.transform.position.y > this.gameObject.transform.position.y - 0.3f && 
					player.transform.position.y < this.gameObject.transform.position.y + 0.3f;

		if(inRange)
		{
			if(this.gameObject.transform.position.x > playerLocation.x && isFacingRight && isDazed == false)
				FlipMonster();
			else if(this.gameObject.transform.position.x < playerLocation.x && isFacingRight == false && isDazed == false)
				FlipMonster();

			if(distanceToPlayer > inRangeToAttackValue && isDazed == false)
			{
				if(this.gameObject.name != Enemies.ShadowMonster)
					shouldDamage = false;
				if(this.gameObject.transform.position.x > player.transform.position.x) //Kogato gleda nalqwo
				{
					this.gameObject.transform.position += new Vector3(-1,0) * Time.deltaTime * sspeed;
				}
				else if(this.gameObject.transform.position.x < player.transform.position.x)// Kogato gleda nadqsno
				{
					this.gameObject.transform.position += new Vector3(1,0) * Time.deltaTime * sspeed;
				}
			}
			else
			{
				if(isDazed == false)
				{
					if(this.gameObject.name != Enemies.ShadowMonster)
						shouldDamage = true;
					DamagePlayer(0); // Hула, за да запали метода просто
				}
			}
		}

		timer += 1 * Time.deltaTime;
	}

	protected virtual void KillMonster()
	{
		PlayerScore.score += this.score;
		this.gameObject.SetActive(false);
	}

	protected abstract void DamagePlayer(int damage);

	protected void FlipMonster()
	{
		this.gameObject.transform.Rotate (Vector3.up, 180);
		if(this.gameObject.transform.FindChild("MonsterHealthBar") != null) // Ротира бара за жизнени точки, ако чудовището има такъв.
			this.gameObject.transform.FindChild("MonsterHealthBar").gameObject.transform.Rotate(Vector3.up, 180);
		isFacingRight = !isFacingRight;
	}

	public virtual void DamageMonster(int damage, bool bigRocket)
	{
		damage -= armor;
		health -= damage;

		if(monsterHurtParticle != null)
		{
			if(this.gameObject.name == Enemies.ShadowMonster)
				Instantiate(monsterHurtParticle, this.gameObject.transform.position + new Vector3(0,-0.5f,-0.11f), Quaternion.Euler(0,0,0));
			else
				Instantiate(monsterHurtParticle, this.gameObject.transform.position + new Vector3(0,0,-0.11f), Quaternion.Euler(-90,0,0));
		}

		DamageMonsterFloatingText (damage);
		UpdateMonsterHealthBar ();
	}

	public virtual void CriticalDamageMonster(int damage, bool bigRocket)
	{
		if(this.gameObject.name == Enemies.ShadowMonster)
			damage += 4;
		else
			damage += (damage / 2);

		damage -= armor;
		health -= damage;

		if(monsterHurtParticle != null)
		{
			if(this.gameObject.name == Enemies.ShadowMonster)
				Instantiate(monsterHurtParticle, this.gameObject.transform.position + new Vector3(0,-0.5f,-0.1f), Quaternion.Euler(0,0,0));
			else
				Instantiate(monsterHurtParticle, this.gameObject.transform.position + new Vector3(0,0,-0.1f), Quaternion.Euler(-90,0,0));
		}

		CriticalDamageMonsterFloatingText (damage);
		UpdateMonsterHealthBar ();
	}

	private void CriticalDamageMonsterFloatingText(int damage)
	{
		FloatingText.Show("-" + damage.ToString()+" КРИТИЧЕН", 
		                  "PlayerDamage", 
		                  new FromWorldToPointTextPositioner(
							GameObject.Find("MainCamera").GetComponent<Camera>(), 
							this.gameObject.transform.position + new Vector3(0,0.5f,0), 
							2f, 
							50f));
	}

	private void DamageMonsterFloatingText(int damage)
	{
		FloatingText.Show("-" + damage.ToString(), 
          		                    	"PlayerDamage", 
              		                    new FromWorldToPointTextPositioner(
		                  					GameObject.Find("MainCamera").GetComponent<Camera>(), 
		                  					this.gameObject.transform.position + new Vector3(0,0.5f,0), 
		                  					2f, 
		                  					50f));
	}

	protected virtual void OnCollisionStay2D(Collision2D col)
	{
		shouldDamage = true;
	}

	protected virtual void OnCollisionExit2D(Collision2D col)
	{
		shouldDamage = false;
	}
	
	protected virtual void SetSpeed(float newSpeed)
	{
		speed = newSpeed;

		if(animatorSpeedVariable != null && monsterAnimator != null && this.gameObject.activeInHierarchy)
			monsterAnimator.SetFloat(animatorSpeedVariable, speed);
	}

	protected IEnumerator MonsterSelfDazer()
	{
		isDazed = true;
		monsterAnimator.SetBool ("hit", true);
		
		timer = 0;
		yield return new WaitForSeconds (3.1f);
		if(timer > 3)
		{
			isDazed = false;
			monsterAnimator.SetBool ("hit", false);

			if(this.gameObject.name == "BlueMonster")
				canDamage = true;
		}
	}

	private void SetAllCollisionIngor()
	{
		GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject[] spikeMonsters = GameObject.FindGameObjectsWithTag("Spikes");
		BoxCollider2D thisBoxCol = this.GetComponent<BoxCollider2D> ();
		CircleCollider2D thisCircleCollider = this.GetComponent<CircleCollider2D> ();
		
		for (int i = 0; i < enemyObjects.Length; i++) 
		{
			if(thisBoxCol != null)
			{
				Physics2D.IgnoreCollision(thisBoxCol, enemyObjects[i].GetComponent<BoxCollider2D>());
				Physics2D.IgnoreCollision(thisBoxCol, enemyObjects[i].GetComponent<CircleCollider2D>());
			}
			if(thisCircleCollider != null)
			{
				Physics2D.IgnoreCollision(thisCircleCollider, enemyObjects[i].GetComponent<BoxCollider2D>());
				Physics2D.IgnoreCollision(thisCircleCollider, enemyObjects[i].GetComponent<CircleCollider2D>());
			}
		}
		
		for (int i = 0; i < spikeMonsters.Length; i++) 
		{
			if(thisBoxCol != null)
			{
				Physics2D.IgnoreCollision(thisBoxCol, spikeMonsters[i].GetComponent<BoxCollider2D>());
			}
			if(thisCircleCollider != null)
			{
				Physics2D.IgnoreCollision(thisCircleCollider, spikeMonsters[i].GetComponent<BoxCollider2D>());
			}
		}
		
		if(thisBoxCol != null)
		{
			Physics2D.IgnoreCollision(thisBoxCol, player.GetComponent<BoxCollider2D>());
			Physics2D.IgnoreCollision(thisBoxCol, player.GetComponent<CircleCollider2D>());
		}
		
		if(thisCircleCollider != null)
		{
			Physics2D.IgnoreCollision(thisCircleCollider, player.GetComponent<BoxCollider2D>());
			Physics2D.IgnoreCollision(thisCircleCollider, player.GetComponent<CircleCollider2D>());
		}
	}

	protected void UpdateMonsterHealthBar()
	{
		if(this.monsterHealthBar == null)
			return;

		float oldX = monsterHealthBar.GetComponent<SpriteRenderer>().bounds.min.x;
	
		monsterHealthBar.transform.localScale = new Vector3 (
				this.health * 0.01f,
			monsterHealthBar.transform.localScale.y,
			monsterHealthBar.transform.localScale.z);
	
		float newX = monsterHealthBar.GetComponent<SpriteRenderer> ().bounds.min.x;
	
		monsterHealthBar.transform.Translate (new Vector3(oldX - (newX - 0.01f), 0f, 0f));
	}
}
