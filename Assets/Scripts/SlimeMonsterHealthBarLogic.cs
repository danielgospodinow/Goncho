using UnityEngine;
using System.Collections;

public class SlimeMonsterHealthBarLogic : MonoBehaviour 
{
	private Transform parent;

	void Start()
	{
		parent = this.gameObject.transform.parent;
	}

	void Update()
	{
		this.gameObject.transform.localScale = new Vector3 (1 / parent.localScale.x,
		                                                    1 / parent.localScale.y,
		                                                    1 / parent.localScale.z);
	}
}
