using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayState : GameState {
	
	private float nextBusSpawnDelay = 15f;
	private float nextSweeperSpawnDelay = 12f;
	private float nextTreasureSpawnDelay = 14f;
	private float restartDelay;

	private float gameTime = 0f;

	private const int MAX_AI = 50;
	private const int NUM_COSTUMES = 4;

	public override void Enter ()
	{
		gameTime = 0f;
		
		nextBusSpawnDelay = nextBusSpawnDelay * (UnityEngine.Random.value + .75f);
		nextSweeperSpawnDelay = nextSweeperSpawnDelay * (UnityEngine.Random.value + .75f);
		nextTreasureSpawnDelay = nextTreasureSpawnDelay * (UnityEngine.Random.value + .75f);

		GameManager.Instance.SetupUI.SetActive(false);
		GameManager.Instance.GameUI.SetActive(true);

		ResourceManager.Instance.GenerateLimitedCostumeList(NUM_COSTUMES);
		for (int i=0; i < 20; i++)
		{
			GameObject spawn = (GameObject) GameObject.Instantiate(Resources.Load<GameObject>("AI"));
			spawn.transform.localPosition = new Vector3(UnityEngine.Random.Range (-8f, 8f), UnityEngine.Random.Range (-3f, 3f));
			spawn.GetComponent<Actor>().CostumeName = ResourceManager.Instance.GetRandomCostumeFromLimitedList();
		}

		for (int i=0; i < 4; i++)
		{
			if (GameManager.Instance.participatingPlayers[i])
			{
				Player p = GameManager.Instance.SpawnPlayer(i);
				p.GetComponent<Actor>().CostumeName = ResourceManager.Instance.GetRandomCostumeFromLimitedList();
				switch (p.PlayerNum)
				{
				case 0:
					p.transform.position = new Vector3(-3.8f + UnityEngine.Random.Range(-3f, 3f), 2.25f + UnityEngine.Random.Range(-2f, 2f));
					break;
				case 1:
					p.transform.position = new Vector3(3.8f + UnityEngine.Random.Range(-3f, 3f), 2.25f + UnityEngine.Random.Range(-2f, 2f));
					break;
				case 2:
					p.transform.position = new Vector3(-3.8f + UnityEngine.Random.Range(-3f, 3f), -2.25f + UnityEngine.Random.Range(-2f, 2f));
					break;
				case 3:
					p.transform.position = new Vector3(3.8f + UnityEngine.Random.Range(-3f, 3f), -2.25f + UnityEngine.Random.Range(-2f, 2f));
					break;
				}
			}
		}

		// Pillars
		
		GameObject pillarSpawn;
		pillarSpawn = (GameObject) GameObject.Instantiate(Resources.Load<GameObject>("Pillar"));
		pillarSpawn.transform.position = new Vector3(-3.8f + UnityEngine.Random.value-.5f, 2.25f + UnityEngine.Random.value-.5f);
		pillarSpawn.renderer.sortingOrder = (int)(-pillarSpawn.transform.position.y*100f)+20;
		pillarSpawn = (GameObject) GameObject.Instantiate(Resources.Load<GameObject>("Pillar"));
		pillarSpawn.transform.position = new Vector3(3.8f + UnityEngine.Random.value-.5f, 2.25f + UnityEngine.Random.value-.5f);
		pillarSpawn.renderer.sortingOrder = (int)(-pillarSpawn.transform.position.y*100f)+20;
		pillarSpawn = (GameObject) GameObject.Instantiate(Resources.Load<GameObject>("Pillar"));
		pillarSpawn.transform.position = new Vector3(-3.8f + UnityEngine.Random.value-.5f, -2.25f + UnityEngine.Random.value-.5f);
		pillarSpawn.renderer.sortingOrder = (int)(-pillarSpawn.transform.position.y*100f)+20;
		pillarSpawn = (GameObject) GameObject.Instantiate(Resources.Load<GameObject>("Pillar"));
		pillarSpawn.transform.position = new Vector3(3.8f + UnityEngine.Random.value-.5f, -2.25f + UnityEngine.Random.value-.5f);
		pillarSpawn.renderer.sortingOrder = (int)(-pillarSpawn.transform.position.y*100f)+20;

		GameManager.Instance.WinLabel.gameObject.SetActive(false);

		GameManager.Instance.OnGameWon += onGameWon;
	}

	private void onGameWon()
	{
		restartDelay = 5.0f;
	}

	public override void Update()
	{
		gameTime += Time.deltaTime;
		int maxAIs = (int) Math.Max ((1 - Mathf.Min (Mathf.Max ((gameTime - 60) / 180f, 0), 1f)) * MAX_AI, 3);
		if (GameManager.Instance.currentBus == null &&
		    GameManager.Instance.GetActors().Count < maxAIs)
		{
			nextBusSpawnDelay -= Time.deltaTime;
			if (nextBusSpawnDelay <= 0)
			{
				GameManager.Instance.SpawnBus();
				
				nextBusSpawnDelay = UnityEngine.Random.Range(10f, 20f);
			}
		}

		nextSweeperSpawnDelay -= Time.deltaTime;
		if (nextSweeperSpawnDelay <= 0)
		{
			GameManager.Instance.SpawnSweeper();
			nextSweeperSpawnDelay = (1.2f - Math.Min(gameTime / 180f, 1f)) * 15f;
		}

		if (GameManager.Instance.currentTreasure == null)
		{
			nextTreasureSpawnDelay -= Time.deltaTime;
			if (nextTreasureSpawnDelay <= 0)
			{
				if (GameManager.Instance.SpawnTreasure() != null)
				{
					nextTreasureSpawnDelay = 10f;
				}
			}
		}
		
		if (GameManager.Instance.WinLabel.gameObject.activeInHierarchy)
		{
			if (restartDelay <= 0)
			{
				GameManager.Instance.Restart ();
			}
			else
			{
				restartDelay -= Time.deltaTime;
				//GameManager.Instance.WinLabel.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
				GameManager.Instance.RestartLabel.text = "Restarting in "+(int)Math.Ceiling(restartDelay)+"...";
			}
		}
	}

	public override void Exit()
	{
		GameManager.Instance.OnGameWon -= onGameWon;
	}
}
