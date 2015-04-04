using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayState : GameState {
	
	private float nextBusSpawnDelay = 15f;
	private float restartDelay;

	private const int NUM_COSTUMES = 4;

	public override void Enter ()
	{
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
				GameManager.Instance.SpawnPlayer(i);
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
		if (GameManager.Instance.currentBus == null && GameManager.Instance.GetActors().Count < 50)
		{
			nextBusSpawnDelay -= Time.deltaTime;
			if (nextBusSpawnDelay <= 0)
			{
				GameManager.Instance.SpawnBus();
				
				nextBusSpawnDelay = UnityEngine.Random.Range(10f, 20f);
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
