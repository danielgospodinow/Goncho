using UnityEngine;
using System.Collections;

public class HealthBarLogic : MonoBehaviour 
{
	private int health;
	private SpriteRenderer[] healthBarSpriteRenderer;
	private Vector3 healthScale;
	private GameObject player;
	public AudioClip playerHurdSound;
	public AudioClip playerDeathSound;
	public static bool alreadyDead = false;
	private float shieldTimer = 0;
	public int shieldAmount = 0;
	private float shieldDuration = 0;
	private bool shieldOn = false;
	public ParticleSystem playerHurtParticle;
	public ParticleSystem playerHealParticle;
	private bool canOuch = true;
	private float ouchTimer = 4f;
	private Vector3 shieldScale;

	public int Health
	{
		get {return this.health;}
	}

	void Awake()
	{
		alreadyDead = false;
		health = 100;
		healthBarSpriteRenderer = GetComponentsInChildren<SpriteRenderer> ();
		healthScale = healthBarSpriteRenderer[0].transform.localScale;
		shieldScale = healthBarSpriteRenderer [1].transform.localScale;
		player = GameObject.FindGameObjectWithTag ("Player");
		UpdateShieldBar ();
	}

	void Update()
	{
		if(alreadyDead)
			return;

		if(canOuch == false)
		{
			ouchTimer += 1 * Time.deltaTime;
			if(ouchTimer > 1.5f)
			{
				canOuch = true;
			}
		}

		shieldTimer += 1 * Time.deltaTime;
		if(shieldTimer > shieldDuration && shieldOn)
		{
			shieldAmount = 0;
			shieldOn = false;
			UpdateShieldBar();
		}

		if(health <= 0 && alreadyDead == false)
		{
			if(Application.loadedLevelName == "MainMenu")
			{
				Destroy(player.gameObject);
				Destroy(gameObject);
			}

			health = 0;
			UpdateHealthBar();
			if(Application.loadedLevelName != "MainMenu")
				AudioSource.PlayClipAtPoint(playerDeathSound, player.transform.position);
			player.GetComponent<Animator>().SetTrigger("animDeath");
			player.GetComponent<BoxCollider2D>().size = new Vector2(player.GetComponent<BoxCollider2D>().size.x, 0.18f);
			player.GetComponent<Animator>().SetFloat("animSpeed", 0f);
			player.GetComponent<PlayerMovement>().OnStopMovingClick();
			player.GetComponent<Animator>().SetBool("animShooting", false);
			player.GetComponent<Rigidbody2D>().isKinematic = false;
			player.GetComponent<PlayerAnimationControl>().playerShooting = false;
			Destroy(GameObject.Find("MainCamera").GetComponent<CameraFollow>());
			Destroy(player.gameObject,4f);
			Destroy(gameObject,4f);
			alreadyDead = true;
			Invoke("ShowDeathMenu", 2f);
		}
	}

	private void ShowDeathMenu ()
	{
		Time.timeScale = 1; //Връщаме нормалната стойност на timeScale-а преди да покаже DeathMenu сцената
		Application.LoadLevel ("DeathMenu");
	}

	public void HealPlayer (int heal)
	{

		Instantiate (playerHealParticle, new Vector3(player.transform.position.x,
		                                             player.transform.position.y,
		                                             player.transform.position.z - 5f), Quaternion.Euler(-90f,0,0));
		if(health == 100)
		{
			return;
		}
		else if(health + heal > 100)
		{
			int toMaxHealth = 100 - health;
			health += toMaxHealth;
			ShowHealText(toMaxHealth.ToString());
		}
		else
		{
			health += heal;
			ShowHealText(heal.ToString());
		}

		UpdateHealthBar ();
	}

	public void ShieldPlayer(int shield, float duration)
	{
		if(shield + shieldAmount <= 100)
		{
			shieldAmount += shield;
		}
		else
		{
			shield = 100 - shieldAmount;
			shieldAmount += shield;
		}

		ShowShieldText (shield.ToString ());
		shieldDuration = duration;
		shieldOn = true;
		shieldTimer = 0;
		UpdateShieldBar ();
	}

	public void DamagePlayer(int damage)
	{
		if(shieldOn == false)
		{
			ShowDamageText("-" +damage.ToString());
			health -= damage;
		}
		else if(shieldOn && damage < shieldAmount)
		{
			shieldAmount -= damage;
			ShowDamageText("(" + damage.ToString() + " АБСОРБИРАН)");
		}
		else if(shieldOn && damage >= shieldAmount)
		{
			shieldOn = false;
			damage -= shieldAmount;
			ShowDamageText("-" +damage.ToString() + "(" + shieldAmount.ToString() + " АБСОРБИРАН)");
			shieldAmount = 0;
			health -= damage;
			UpdateShieldBar();
		}

		if(canOuch)
		{
			if(Application.loadedLevelName != "MainMenu")
				AudioSource.PlayClipAtPoint (playerHurdSound, player.transform.position);
			ouchTimer = 0f;
			canOuch = false;
		}

		Instantiate (playerHurtParticle, player.transform.position, player.transform.rotation);
		StartCoroutine ("DamageEffects", 0.4f);
		UpdateHealthBar ();
		UpdateShieldBar ();
	}

	private void ShowShieldText(string text)
	{
		FloatingText.Show("+" + text, 
		                  "Shield", 
		                  new FromWorldToPointTextPositioner(
			GameObject.Find("MainCamera").GetComponent<Camera>(), 
			this.gameObject.transform.position, 
			2f, 
			50f));
	}

	private void ShowHealText(string text)
	{
		FloatingText.Show("+" + text, 
		                  "Heal", 
		                  new FromWorldToPointTextPositioner(
			GameObject.Find("MainCamera").GetComponent<Camera>(), 
			this.gameObject.transform.position, 
			2f, 
			50f));
	}

	private void ShowDamageText(string text)
	{
		FloatingText.Show(text, 
		                  "Damage", 
		                  new FromWorldToPointTextPositioner(
								GameObject.Find("MainCamera").GetComponent<Camera>(), 
								this.gameObject.transform.position, 
								2.5f, 
								50f));
	}

	private Color32 pink = new Color32(255,124,124,255);
	private Color32 white = new Color32 (255, 255, 255, 255);

	private IEnumerator DamageEffects(float seconds)
	{
		for (int i = 0; i < 6; i++) {
			if(i % 2 == 0)
			{
				player.GetComponent<SpriteRenderer>().color = Color.Lerp(white, pink, 1);
				yield return new WaitForSeconds (seconds);
			}
			else
			{
				player.GetComponent<SpriteRenderer>().color = Color.Lerp(pink, white, 1);
				yield return new WaitForSeconds (seconds);
			}
		}
	}
	
	private void UpdateHealthBar ()
	{
		float originalValue = healthBarSpriteRenderer[0].GetComponent<Renderer>().bounds.min.x;
		healthBarSpriteRenderer[0].material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);
		healthBarSpriteRenderer[0].transform.localScale = new Vector3(healthScale.x* health * 0.01f, 0.48f, 1f);
		float newValue = healthBarSpriteRenderer[0].GetComponent<Renderer>().bounds.min.x;
		float difference = newValue - originalValue;
		healthBarSpriteRenderer[0].transform.Translate(new Vector3(-difference, 0f, 0f));
	}

	private void UpdateShieldBar()
	{
		float originalValue = healthBarSpriteRenderer [1].GetComponent<Renderer> ().bounds.min.x;
		healthBarSpriteRenderer [1].material.color = Color.Lerp (Color.gray, Color.gray, 1 - shieldAmount * 0.01f);
		healthBarSpriteRenderer[1].transform.localScale = new Vector3(shieldScale.x * shieldAmount * 0.01f, 0.48f, 1f);
		float newValue = healthBarSpriteRenderer[1].GetComponent<Renderer>().bounds.min.x;
		float difference = newValue - originalValue;
		healthBarSpriteRenderer [1].transform.position = new Vector3 (
			healthBarSpriteRenderer [1].transform.position.x - (difference + 0f /* 2.5f */ * shieldAmount / 25 * 0.001f), 
			healthBarSpriteRenderer [1].transform.position.y, 
			healthBarSpriteRenderer [1].transform.position.z);
	}
}
