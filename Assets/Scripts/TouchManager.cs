using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour 
{
	public static bool guiTouch = false;

	public void TouchInputHandler()
	{
		//For GUI Textures only!
		if(GetComponent<GUITexture>() != null)
		{
			if(GetComponent<GUITexture>().HitTest(Input.GetTouch(0).position))
			{
				guiTouch = true;
				switch(Input.GetTouch(0).phase)
				{
				case TouchPhase.Began:
					SendMessage("OnFirstTouchBegan");
					SendMessage("OnFirstTouch");
					break;
				case TouchPhase.Moved:
					SendMessage("OnFirstTouchMoved");
					SendMessage("OnFirstTouch");
					break;
				case TouchPhase.Stationary:
					SendMessage("OnFirstTouchStationary");
					SendMessage("OnFirstTouch");
					break;
				case TouchPhase.Ended:
					SendMessage("OnFirstTouchEnded");
					SendMessage("OnFirstTouch");
					guiTouch = false;
					break;
				}
			}
			if(GetComponent<GUITexture>().HitTest(Input.GetTouch(1).position))
			{
				guiTouch = true;
				switch(Input.GetTouch(1).phase)
				{
				case TouchPhase.Began:
					SendMessage("OnSecondTouchBegan");
					SendMessage("OnSecondTouch");
					break;
				case TouchPhase.Moved:
					SendMessage("OnSecondTouchMoved");
					SendMessage("OnSecondTouch");
					break;
				case TouchPhase.Stationary:
					SendMessage("OnSecondTouchStationary");
					SendMessage("OnSecondTouch");
					break;
				case TouchPhase.Ended:
					SendMessage("OnSecondTouchEnded");
					SendMessage("OnSecondTouch");
					guiTouch = false;
					break;
				}
			}
		}
	}
}
