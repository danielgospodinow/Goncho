using UnityEngine;
using System.Collections;

public class BirdLogic : MonoBehaviour 
{
	public bool isFacingLeft;
	private int speed;

	void Start()
	{
		speed = Random.Range (1, 4);
	}

	void Update () 
	{
		if(isFacingLeft == false)
			this.transform.position += new Vector3 (speed,0,0) * Time.deltaTime;
		else
			this.transform.position += new Vector3 (-speed,0,0) * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(this.isFacingLeft == false && col.gameObject.name =="RightBound")
		{
			Destroy(this.gameObject);
		}
		else if(this.isFacingLeft && col.gameObject.name =="LeftBound")
		{
			Destroy(this.gameObject);
		}
	}
}
