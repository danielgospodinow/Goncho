using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{
	private Text playerScore;
	private Text playerMoney;

	void Start()
	{
		playerScore = this.gameObject.GetComponentsInChildren<Text> () [1];
		playerMoney = this.gameObject.GetComponentsInChildren<Text> () [2];

		UpdatePlayerScore ();
	}

	void Update()
	{
		UpdatePlayerScore ();
	}

	public void UpdatePlayerScore()
	{
		playerScore.text = "Точки: " + PlayerScore.score.ToString();
		playerMoney.text = "Пари: " + PlayerScore.money.ToString();
	}
}
