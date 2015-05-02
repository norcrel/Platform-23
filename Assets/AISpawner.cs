using UnityEngine;
using System.Collections;

public class AISpawner : MonoBehaviour {
	public float delay = 3f;
	private float initialDelay;

	public int NumToSpawn = 10;

	public static float DELAY_BETWEEN_SPAWNS = .2f;

	private SpriteRenderer _meshRenderer;
	private SpriteRenderer m_meshRenderer {
		get {
			if (_meshRenderer == null) _meshRenderer = GetComponentInChildren<SpriteRenderer>();
			return _meshRenderer;
		}
	}

	private float lastSpawnTime = float.MinValue;
	// Use this for initialization
	void Start () {
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 20);
		initialDelay = delay;
		m_meshRenderer.color = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
		if (delay > 0)
		{
			delay -= Time.deltaTime;

			float normalizedArrivalTime = 1 - delay / initialDelay;
			if (normalizedArrivalTime < .8f)
			{
				normalizedArrivalTime = 0;
			}
			else
			{
				normalizedArrivalTime = (normalizedArrivalTime-.8f) / .2f;
			}
			m_meshRenderer.color = new Color(normalizedArrivalTime, normalizedArrivalTime, normalizedArrivalTime);
			return;
		}

		if (Time.time - lastSpawnTime > DELAY_BETWEEN_SPAWNS)
		{
			Spawn ();
		}
	}

	void Spawn()
	{
		lastSpawnTime = Time.time;

		GameObject spawn = (GameObject) Instantiate(Resources.Load<GameObject>("AI"));
		spawn.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y);
		Vector3 targetPos = Random.onUnitSphere * Random.Range(.2f, 3f);
		targetPos += spawn.transform.localPosition;
		targetPos.z = 0;

		spawn.GetComponent<AI>().SetTargetPosition(targetPos);
		
		NumToSpawn--;
		
		if (NumToSpawn <= 0)
		{
			Destroy (gameObject);
		}
	}
}
