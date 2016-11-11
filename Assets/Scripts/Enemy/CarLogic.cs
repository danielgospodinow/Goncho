using UnityEngine;
using System.Collections;

public class CarLogic : MonoBehaviour 
{
	private GameObject player;
	private Vector3 playerLocation;
	private float distanceToPlayer;
	private bool inRange;
	private float inRangeValue = 7f;
	private bool isFacingRight;
	private Rigidbody2D carRigid;
	private float speed = 25f;
	private bool onceRotater = false;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerLocation = GameObject.FindGameObjectWithTag ("Player").transform.position;
		isFacingRight = true;
		carRigid = this.gameObject.GetComponent<Rigidbody2D> ();
	}

	void Update()
	{
		if(HealthBarLogic.alreadyDead)
		{
			speed = 0;
			this.gameObject.GetComponent<Animator> ().SetFloat ("carSpeed", speed);
			return;
		}

		playerLocation = player.transform.position;
		distanceToPlayer = Vector3.Distance (this.gameObject.transform.position, player.gameObject.transform.position);
		this.gameObject.GetComponent<Animator> ().SetFloat ("carSpeed", speed);

		if(inRange == false)
		{
			inRange = distanceToPlayer < inRangeValue && 
				player.transform.position.y > this.gameObject.transform.position.y - 0.3f && 
					player.transform.position.y < this.gameObject.transform.position.y + 0.3f;
		}

		if(inRange)
		{
			if(this.gameObject.transform.position.x > playerLocation.x && isFacingRight)
				FlipCar();
			else if(this.gameObject.transform.position.x < playerLocation.x && isFacingRight == false)
				FlipCar();
			else
				onceRotater = true;

			StartCoroutine("SpeedIncreaser");

			if(isFacingRight)
				carRigid.AddForce(new Vector2(150,0) * Time.deltaTime * speed);
			else
				carRigid.AddForce(new Vector2(-150,0) * Time.deltaTime * speed);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			if(player.GetComponent<Animator>().GetBool("animKneeing"))
			{
				player.GetComponent<PlayerAnimationControl>().OnUpButtonClick();
				player.GetComponent<Rigidbody2D> ().isKinematic = false;
				player.GetComponent<Rigidbody2D> ().AddForce (new Vector2(player.transform.position.x - this.gameObject.transform.position.x, player.transform.position.y - this.gameObject.transform.position.y) * 3000);
			}
		}
	}

	void OnCollisionStay2D(Collision2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			if(player.GetComponent<Animator>().GetBool("animKneeing"))
			{
				player.GetComponent<PlayerAnimationControl>().OnUpButtonClick();
				player.GetComponent<Rigidbody2D> ().AddForce (new Vector2(player.transform.position.x - this.gameObject.transform.position.x, player.transform.position.y - this.gameObject.transform.position.y) * 3000);
			}
		}
	}

	private IEnumerator SpeedIncreaser()
	{
		while(speed <= 50)
		{
			yield return new WaitForSeconds(1f);
			speed += 1f;
			yield return new WaitForSeconds(1f);
		}
	}

	private void FlipCar()
	{
		if(onceRotater)
			return;

		onceRotater = true;
		this.gameObject.transform.Rotate (Vector3.up, 180);
		isFacingRight = !isFacingRight;
	}
}
