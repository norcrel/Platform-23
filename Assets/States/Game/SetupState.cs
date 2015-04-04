using UnityEngine;

public class SetupState : GameState {
	public override void Enter ()
	{
		GameManager.Instance.SetupUI.SetActive(true);
		GameManager.Instance.GameUI.SetActive(false);
		GameManager.Instance.ClearAllActors();
	}

	private int fadeDirection = 1;
	
	public override void Update()
	{
		bool wantsStart = false;
		for (int player=0; player < 4; player++)
		{
			bool joinPressed = Input.GetButtonDown ("BottomButton" + player);
			bool cancelPressed = Input.GetButtonDown ("RightButton" + player);

			if (joinPressed)
			{
				GameManager.Instance.PlayerJoined(player);
			}
			else if (cancelPressed)
			{
				GameManager.Instance.PlayerLeft(player);
				Player p = GameManager.Instance.GetPlayer(player);
				if (p != null) GameObject.Destroy(p.gameObject);
			}

			if (GameManager.Instance.participatingPlayers[player] && GameManager.Instance.GetPlayer(player) == null)
			{
				Player spawnedPlayer = GameManager.Instance.SpawnPlayer(player);
				switch (player)
				{
				case 0:
					spawnedPlayer.transform.position = new Vector3(-3.8f, 2.25f);
					break;
				case 1:
					spawnedPlayer.transform.position = new Vector3(3.8f, 2.25f);
					break;
				case 2:
					spawnedPlayer.transform.position = new Vector3(-3.8f, -2.25f);
					break;
				case 3:
					spawnedPlayer.transform.position = new Vector3(3.8f, -2.25f);
					break;
				}
			}

			wantsStart = wantsStart || Input.GetButton ("Start" + player) || Input.GetButton ("Logo" + player) || Input.GetButton ("Pad" + player);
		}
		
		Color c = GameManager.Instance.StartInstructions.renderer.material.color;
		if (GameManager.Instance.GetNumPlayers() >= 1)
		{
			if (fadeDirection == 1 && c.a > 1)
			{
				fadeDirection = -1;
			} else if (fadeDirection == -1 && c.a < 0)
			{
				fadeDirection = 1;
			}

			c.a += fadeDirection * Time.deltaTime*2;

			if (wantsStart)
			{
				GameManager.Instance.GetComponent<StateMachine>().ChangeState(GameManager.Instance.GamePlayState);
			}
		}
		else
		{
			c.a = 0;
		}
		GameManager.Instance.StartInstructions.renderer.material.color = c;
	}
	
	public override void Exit()
	{
		GameManager.Instance.SetupUI.SetActive(false);
		
		GameManager.Instance.ClearAllActors();
	}
}
