using UnityEngine;
using System.Collections;

public class TiltScript : MonoBehaviour {
	void Update () {
		Vector3 dir = Vector3.zero;
		dir.x = -Input.acceleration.x;
		transform.position += new Vector3(-dir.x, 0f) * Time.deltaTime * 0.3f;
	}
}
