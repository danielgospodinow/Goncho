using UnityEngine;
using System.Collections;

public class JortLogic : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,700));
		}
	}
}
