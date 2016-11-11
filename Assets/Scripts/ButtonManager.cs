using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonManager : MonoBehaviour
{
	private TextAlert textAlert;
	public GameObject rocketLauncher;

	void Awake()
	{
		//Инстанция още в началото на FloatingText, защото по-късната му инстанция дава лаг спайк.
		if (Application.loadedLevelName == "MainMenu")
			FloatingText.Show(string.Empty, 
			                  "Shield", 
			                  new FromWorldToPointTextPositioner(GameObject.Find("MainCamera").GetComponent<Camera>(), 
								 							     Vector3.one, 
								 								 2f, 
								 								 50f));
	}

	private IEnumerator Level5Intro()
	{
		yield return new WaitForSeconds (1f);
		textAlert.ShowMessage("Вие стигнахте до края на играта! Успех с чудовището пред вас!");
	}

	void Start()
	{
		if(Application.loadedLevelName == "LevelCompleteScene" || Application.loadedLevelName == "MainMenu" || Application.loadedLevelName == "DeathMenu" || Application.loadedLevelName == "InformationScene" || Application.loadedLevelName == "GameCompleteScene")
		{
			return;
		}

		GameObject player = GameObject.Find ("Player");
		PlayerMovement playerMovementScript = player.GetComponent<PlayerMovement> ();
		PlayerAnimationControl playerAnimControlScript = player.GetComponent<PlayerAnimationControl> ();
		textAlert = this.gameObject.GetComponentInChildren<TextAlert> ();
		HealthBarLogic playerHealthBar = GameObject.Find ("HealthBar").GetComponent<HealthBarLogic> ();

		if(Application.loadedLevelName == "Level5")
		{
			StartCoroutine(Level5Intro());
		}

		Button[] buttons = gameObject.GetComponentsInChildren<Button> ();

//		buttons [0].onClick.AddListener (() => {
//			//playerMovementScript.OnLeftButtonClick();
//		});

		buttons [1].onClick.AddListener (() => {
			playerAnimControlScript.OnButtonDownClick();
		});

//		buttons [2].onClick.AddListener (() => {
//			//playerMovementScript.OnRightButtonClick();
//		});

		buttons [3].onClick.AddListener (() => {
			playerAnimControlScript.OnUpButtonClick();
		});

		buttons [4].onClick.AddListener (() => {
			playerMovementScript.OnStopMovingClick();
		});

		buttons [5].onClick.AddListener (() => {
			playerAnimControlScript.OnShootButtonClick();
		});

		buttons [6].onClick.AddListener (() => {
			playerMovementScript.OnJumpButtonClick();
		});

		buttons [7].onClick.AddListener (() => {
			//ShieldBuy
			if(HealthBarLogic.alreadyDead)
				return;

			if(PlayerScore.money >= 15)
			{
				if(playerHealthBar.shieldAmount < 100)
				{
					playerHealthBar.ShieldPlayer(50,6);
					PlayerScore.money -= 15;
				}
				else
					textAlert.ShowMessage("Нямате нужда от това в момента!");
			}
			else
				textAlert.ShowMessage("Нямате достатъчно пари, за да закупите това!");
		});

		buttons [8].onClick.AddListener (() => {
			//HealthBuy
			if(HealthBarLogic.alreadyDead)
				return;

			if(PlayerScore.money >= 20)
			{
				if(playerHealthBar.Health == 100)
				{
					textAlert.ShowMessage("Вече сте на пълни жизнени точки!");
					return;
				}

				playerHealthBar.HealPlayer(40);
				PlayerScore.money -= 20;
			}
			else
				textAlert.ShowMessage("Нямате достатъчно пари, за да закупите това!");
			
		});

		buttons [9].onClick.AddListener (() => {
			//TimeSlowerBuy
			if(HealthBarLogic.alreadyDead)
				return;

			if(PlayerScore.money >= 10)
			{
				if(GameObject.Find("TimeSlowerGO") != null)
					textAlert.ShowMessage("Вече сте използвали този ефект!");
				else
				{
					GameObject timeSlowerGO = new GameObject("TimeSlowerGO");
					timeSlowerGO.AddComponent<CollectablesLogic>();
					StartCoroutine(timeSlowerGO.GetComponent<CollectablesLogic>().TimeSlower());
					PlayerScore.money -= 10;
				}
			}
			else
				textAlert.ShowMessage("Нямате достатъчно пари, за да закупите това!");
		});

		buttons [10].onClick.AddListener (() => {
			//RocketLauncherBuy
			if(HealthBarLogic.alreadyDead)
				return;
			
			if(PlayerScore.money >= 50)
			{
				if(playerAnimControlScript.isKneeingButtonClicked)
					playerAnimControlScript.OnUpButtonClick();

				if(playerMovementScript.facingRight)
					Instantiate(rocketLauncher, player.gameObject.transform.position - new Vector3(0,0.1f,0), Quaternion.Euler(0,180,0));
	            else if(playerMovementScript.facingRight == false)
		            Instantiate(rocketLauncher, player.gameObject.transform.position - new Vector3(0,0.1f,0), Quaternion.Euler(0,0,0));

				PlayerScore.money -= 50;
			}
			else
				textAlert.ShowMessage("Нямате достатъчно пари, за да закупите това!");
		});
	}

	public void RestartLevel()
	{
		Application.LoadLevel ("Level" + GameController.currentLevel.ToString());
	}

	public void ExitGame()
	{
		Application.Quit ();
	}

	public void StartGame()
	{
		GameController.currentLevel = 0;
		GameController.currentLevel += 1;
		Application.LoadLevel ("Level" + GameController.currentLevel.ToString());
	}

	public void NextLevel()
	{
		GameController.currentLevel += 1;
		Application.LoadLevel ("Level" + GameController.currentLevel.ToString());
	}

	public void OpenMainMenu()
	{
		Application.LoadLevel ("MainMenu");
	}

	public void OpenInformation()
	{
		Application.LoadLevel ("InformationScene");
	}
}