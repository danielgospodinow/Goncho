using UnityEngine;
using System.Collections;

public class CollectablesLogic : MonoBehaviour
{
	public AudioClip[] collectSound;
	static int currentSound = 0;
	public AudioClip shieldSound;
	public AudioClip coinSound;
	private HealthBarLogic healthBarLogicScript;
	private Vector3 currentPos;
	private Vector3 wantedPos;
	private bool goingUp;
	public float distanceToTravel;
	public float travelingSpeed;
	public bool staticObject;

	void Awake()
	{
		healthBarLogicScript = GameObject.Find ("HealthBar").GetComponent<HealthBarLogic> ();
		this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,
		                                                 this.gameObject.transform.position.y,
		                                                 0.001f);

		goingUp = true;
		currentPos = this.gameObject.transform.position;
		wantedPos = currentPos + new Vector3 (0,0.3f,0);
		distanceToTravel = 1f;
		travelingSpeed = 30f;
	}

	void Update()
	{
		if(staticObject)
			return;

		currentPos = this.gameObject.transform.position;

		if(goingUp)
		{
			if(currentPos.y < wantedPos.y)
			{
				this.gameObject.transform.position += new Vector3 (0, travelingSpeed * 0.01f, 0) * Time.deltaTime;
				currentPos = this.gameObject.transform.position;
			}
			else if(currentPos.y >= wantedPos.y)
			{
				goingUp = false;
				currentPos = wantedPos;
				wantedPos = currentPos - new Vector3(0, distanceToTravel * 0.3f,0);
			}
		}
		else
		{
			if(currentPos.y >= wantedPos.y)
			{
				this.gameObject.transform.position += new Vector3 (0, travelingSpeed * -0.01f, 0) * Time.deltaTime;
				currentPos = this.gameObject.transform.position;
			}
			else if(currentPos.y < wantedPos.y)
			{
				goingUp = true;
				currentPos = wantedPos;
				wantedPos = currentPos + new Vector3(0, distanceToTravel * 0.3f, 0);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			if(this.gameObject.tag == "Healpack")
			{
				if(currentSound > collectSound.Length-1)
				{
					currentSound = 0;
				}

				if(Application.loadedLevelName != "MainMenu")
					AudioSource.PlayClipAtPoint(collectSound[currentSound], transform.position);
				currentSound++;
				healthBarLogicScript.HealPlayer(40);

				this.gameObject.SetActive(false);
			}
			else if(this.gameObject.tag == "Shield")
			{
				if(Application.loadedLevelName != "MainMenu")
					AudioSource.PlayClipAtPoint(shieldSound, transform.position);

				healthBarLogicScript.ShieldPlayer(30, 5f);

				this.gameObject.SetActive(false);
			}
			else if(this.gameObject.tag == "TimeSlower")
			{
				StartCoroutine("TimeSlower");
			}
			else if(this.gameObject.tag == "Coin")
			{
				if(Application.loadedLevelName != "MainMenu")
					AudioSource.PlayClipAtPoint(coinSound, transform.position);

				PlayerScore.money += 15;
				
				FloatingText.Show("+15", 
				                  "MonsterDamage", 
				                  new FromWorldToPointTextPositioner(
											   GameObject.Find("MainCamera").GetComponent<Camera>(), 
			                                   this.gameObject.transform.position, 
			                                   2f, 
			                                   50f));


				this.gameObject.SetActive(false);
			}
		}
	}

	public IEnumerator TimeSlower()
	{
		if(this.gameObject.GetComponent<BoxCollider2D> () != null)
			this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;

		this.gameObject.transform.position = new Vector3 (
			this.gameObject.transform.position.x, 
			this.gameObject.transform.position.y, 
			100f);
		Time.timeScale = 0.5f;
		yield return new WaitForSeconds(3f);
		Time.timeScale = 1f;

		this.gameObject.SetActive(false);
	}
}
