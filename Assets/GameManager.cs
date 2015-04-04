using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private static GameManager _instance;

	private List<Actor> allActors = new List<Actor>();

	public bool[] participatingPlayers = new bool[] {false, false, false, false};
	private Player[] allPlayers = new Player[] {null, null, null, null};

	public GameObject currentBus = null;

	public Text WinLabel;
	public Text RestartLabel;

	public GameObject GameUI;
	public GameObject SetupUI;

	public GameObject StartInstructions;

	public bool isApplicationQuitting { get { return _isApplicationQuitting; } }
	private bool _isApplicationQuitting = false;

	public State CurrentState { get { return GetComponent<StateMachine>().CurrentState; } }

	public Action OnGameWon;
	
	public SetupState GameSetupState = new SetupState();
	public PlayState GamePlayState = new PlayState();

	public static GameManager Instance
	{
		get { return _instance; }
	}

	[HideInInspector]
	public int[] Scores = new int[4];

	void Awake () {
		_instance = this;
	}

	void Start()
	{
		GetComponent<StateMachine>().ChangeState(GameSetupState);
	}
	
	public int GetNumPlayers()
	{
		int numPlayers = 0;
		for (int i=0; i < 4; i++)
		{
			if (participatingPlayers[i]) numPlayers++;
		}
		
		return numPlayers;
	}
	
	public int GetNumLivingPlayers()
	{
		int numPlayers = 0;
		for (int i=0; i < 4; i++)
		{
			if (allPlayers[i] != null) numPlayers++;
		}
		
		return numPlayers;
	}

	public void ClearAllActors()
	{
		for (int i=allActors.Count-1; i >= 0; i--)
		{
			Destroy(allActors[i].gameObject);
		}
		allActors.Clear();

		for (int i=0; i < 4; i++)
		{
			allPlayers[i] = null;
		}
	}

	public void RegisterActor(Actor a)
	{
		allActors.Add(a);
		Player p = a.GetComponent<Player>();
		if (p != null)
		{
			allPlayers[p.PlayerNum] = p;
		}
	}

	public void UnregisterActor(Actor a)
	{
		allActors.Remove(a);
		Player p = a.GetComponent<Player>();
		if (p != null)
		{
			int numPlayersAlive = 0;
			Player lastPlayer = null;
			for (int i=0; i < 4; i++)
			{
				if (allPlayers[i] != null && !allPlayers[i].GetComponent<Actor>().Killed)
				{
					numPlayersAlive++;
					lastPlayer = allPlayers[i];
				}
			}

			if (numPlayersAlive == 1)
			{
				Win(lastPlayer);
			}
		}
	}

	public void PlayerJoined(int index)
	{
		if (!participatingPlayers[index])
		{
			participatingPlayers[index] = true;

			SoundManager.Instance.PlaySound("joinSFX");
		}
	}

	public void PlayerLeft(int index)
	{
		if (participatingPlayers[index])
		{
			participatingPlayers[index] = false;

			SoundManager.Instance.PlaySound("quitSFX");
		}
	}

	public Player SpawnPlayer(int playerNum)
	{
		GameObject playerObj = (GameObject) Instantiate (Resources.Load<GameObject>("Player"));
		Player player = playerObj.GetComponent<Player>();
		player.PlayerNum = playerNum;

		Actor playerActor = playerObj.GetComponent<Actor>();
		playerActor.FaceRandom();

		return player;
	}

	public void Win(Player p)
	{
		WinLabel.gameObject.SetActive(true);

		WinLabel.text = "P"+p.PlayerNum+" Win!";
		RestartLabel.text = "Restarting in 5...";

		SoundManager.Instance.PlaySound("winSFX");

		if (OnGameWon != null) OnGameWon();
	}

	public List<Actor> GetActors()
	{
		return allActors;
	}

	public void SpawnBus()
	{
		currentBus = (GameObject) Instantiate(Resources.Load<GameObject>("Spawner"));
		currentBus.transform.localPosition = new Vector3(UnityEngine.Random.Range (-8f, 8f), UnityEngine.Random.Range (-3f, 3f));
		AISpawner ais = currentBus.GetComponent<AISpawner>();
		ais.NumToSpawn = UnityEngine.Random.Range(5, 25);
		ais.delay = UnityEngine.Random.Range (2,5);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			Restart ();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public Player GetPlayer(int playerNum)
	{
		return allPlayers[playerNum];
	}

	public void Restart()
	{
		Application.LoadLevel(0);
	}

	void OnApplicationQuit () {
		_isApplicationQuitting = true;
	}
}
