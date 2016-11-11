using UnityEngine;
using System.Collections;

public class ShowAdOnLoad : MonoBehaviour 
{
	public string zoneID;
	public bool useTimeout = true;
	public float timeoutDuration = 15f;
	public float yieldTime = 0.5f;
	
	private float _startTime = 0f;
	
	#if UNITY_IOS || UNITY_ANDROID
	// A return type of IEnumerator allows for the use of yield statements.
	//  For more info, see: http://docs.unity3d.com/ScriptReference/YieldInstruction.html
	IEnumerator Start ()
	{
		// Zone name used in debug messages.
		string zoneName = string.IsNullOrEmpty(zoneID) ? "the default ad placement zone" : zoneID;
		
		// Set a start time for the timeout.
		_startTime = Time.timeSinceLevelLoad;
		
		// Check to see if Unity Ads is initialized.
		//  If not, wait a second before trying again.
		while (!UnityAdsHelper.isInitialized)
		{
			if (useTimeout && Time.timeSinceLevelLoad - _startTime > timeoutDuration)
			{
				Debug.LogWarning(string.Format("Unity Ads failed to initialize in a timely manner. " +
				                               "An ad for {0} will not be shown on load.",zoneName));
				
				// Break out of both this loop and the Start method; Unity Ads will not
				//  be shown on load since the wait time exceeded the time limit.
				yield break;
			}
			
			yield return new WaitForSeconds(yieldTime);
		}
		
		Debug.Log("Unity Ads has finished initializing. Waiting for ads to be ready...");
		
		// Set a start time for the timeout.
		_startTime = Time.timeSinceLevelLoad;
		
		// Check to see if ads are available and ready to be shown. 
		//  If ads are not available, wait before trying again.
		while (!UnityAdsHelper.IsReady(zoneID))
		{
			if (useTimeout && Time.timeSinceLevelLoad - _startTime > timeoutDuration)
			{
				Debug.LogWarning(string.Format("Unity Ads failed to be ready in a timely manner. " +
				                               "An ad for {0} will not be shown on load.",zoneName));
				
				// Break out of both this loop and the Start method; Unity Ads will not
				//  be shown on load since the wait time exceeded the time limit.
				yield break;
			}
			
			yield return new WaitForSeconds(yieldTime);
		}
		
		Debug.Log(string.Format("Ads for {0} are available and ready. Showing ad now...",zoneName));
		
		// Now that ads are ready, show an ad campaign.
		UnityAdsHelper.ShowAd(zoneID);
	}
	#endif
}