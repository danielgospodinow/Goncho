using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextAlert : MonoBehaviour 
{
	private Text text;
	private Color32 startColor = new Color32(219,0,0,255);
	private Color32 currentColor;

	void Start()
	{
		text = this.gameObject.GetComponent<Text> ();
		currentColor = startColor;
	}

	public void ShowMessage(string message)
	{
		ClearText ();
		StopAllCoroutines ();
		text.text = message;
		text.color = startColor;
		StartCoroutine ("TextFaderr");
	}

	private void ClearText()
	{
		text.text = string.Empty;
		text.color = startColor;
	}

	private IEnumerator TextFaderr()
	{
		yield return new WaitForSeconds(2f);
		currentColor = text.color;
		while(currentColor.a > 0)
		{
			if(currentColor.a <= 20)
				break;

			currentColor.a -= 5;
			text.color = currentColor;
			currentColor = text.color;
			yield return new WaitForSeconds(0.02f);
		}
		ClearText ();
	}
}
