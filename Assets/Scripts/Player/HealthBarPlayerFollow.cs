using UnityEngine;
using System.Collections;

public class HealthBarPlayerFollow : MonoBehaviour 
{
	private Vector3 offset;			
	private Transform player;	

	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		offset = new Vector3 (0, 0.7f, 0);
	}
	
	void Update ()
	{
		transform.position = player.position + offset;
	}	
}
