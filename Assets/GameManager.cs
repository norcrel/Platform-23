using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	private static GameManager _instance;

	private List<Actor> allActors = new List<Actor>();

	private Player[] allPlayers = new Player[] {null, null, null, null};

	private float nextBusSpawnDelay = 15f;
	private GameObject currentBus = null;

	public Text WinLabel;
	public Text RestartLabel;
	private float restartDelay;

	public bool isApplicationQuitting = false;

	public static GameManager Instance
	{
		get { return _instance; }
	}

	public int[] Scores = new int[4];

	void Awake () {
		_instance = this;

		for (int i=0; i < 20; i++)
		{
			GameObject spawn = (GameObject) Instantiate(Resources.Load<GameObject>("AI"));
			spawn.transform.localPosition = new Vector3(UnityEngine.Random.Range (-8f, 8f), UnityEngine.Random.Range (-3f, 3f));
		}

		WinLabel.gameObject.SetActive(false);
	}

	public void RegisterActor(Actor a)
	{
		allActors.Add(a);
		Player p = a.GetComponent<Player>();
		if (p != null)
		{
			allPlayers[p.PlayerNum] = p;
			
			p.transform.localPosition = new Vector3(UnityEngine.Random.Range (-8f, 8f), UnityEngine.Random.Range (-3f, 3f));
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

	public void Win(Player p)
	{
		WinLabel.gameObject.SetActive(true);

		restartDelay = 5.0f;

		WinLabel.text = "P"+p.PlayerNum+" Win!";
		RestartLabel.text = "Restarting in 5...";
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

		nextBusSpawnDelay = UnityEngine.Random.Range(10f, 20f);
	}

	void Update()
	{
		if (currentBus == null && allActors.Count < 50)
		{
			nextBusSpawnDelay -= Time.deltaTime;
			if (nextBusSpawnDelay <= 0)
			{
				SpawnBus();
			}
		}

		if (Input.GetKeyDown(KeyCode.Backspace))
		{
			Restart ();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if (WinLabel.gameObject.activeInHierarchy)
		{
			if (restartDelay <= 0)
			{
				Restart ();
			}
			else
			{
				restartDelay -= Time.deltaTime;
				WinLabel.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
				RestartLabel.text = "Restarting in "+(int)Math.Ceiling(restartDelay)+"...";
			}
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
		isApplicationQuitting = true;
	}
}
