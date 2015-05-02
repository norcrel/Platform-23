using UnityEngine;

public class SetupState : GameState {
	private Sweeper m_tutorialSweeper;
	private float m_sweeperSpawnTimer = 0f;
	public override void Enter ()
	{
		GameManager.Instance.SetupUI.SetActive(true);
		GameManager.Instance.GameUI.SetActive(false);
		GameManager.Instance.ClearAllActors();
	}

	private int fadeDirection = 1;
	
	public override void Update()
	{
		if (m_tutorialSweeper == null)
		{
			m_sweeperSpawnTimer -= Time.deltaTime;
			if (m_sweeperSpawnTimer <= 0)
			{
				m_tutorialSweeper = GameManager.Instance.SpawnSweeper();
				m_tutorialSweeper.transform.localPosition = new Vector3(0f, 1.75f, 0f);
				m_tutorialSweeper.SweepTime = float.MaxValue;
				m_sweeperSpawnTimer = 3f;
			}
		}

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

		if (Input.GetKeyDown(KeyCode.T))
		{
			GameManager.Instance.PlayWithTreasure = !GameManager.Instance.PlayWithTreasure;
		}
	}
	
	public override void Exit()
	{
		GameManager.Instance.SetupUI.SetActive(false);
		
		GameManager.Instance.ClearAllActors();
	}
}
