using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {

	public float Speed = .3f; // Movement speed

	public float AttackRange = 1f; // Attack and interaction range

	private bool m_killed = false;
	private float killedTime;
	private const float DESTROY_TIME = 3.0f;
	public bool Killed { get { return m_killed; } }

	// Use this for initialization
	void Start () {
		GameManager.Instance.RegisterActor(this);
	}
	
	// Update is called once per frame
	void Update () {
		if (m_killed)
		{
			float dt = Time.time - killedTime;
			if (dt > 1.0)
			{
				MeshRenderer r = gameObject.GetComponentInChildren<MeshRenderer>();
				Color c = r.material.color;
				c.a = 1 - dt / DESTROY_TIME;
				r.material.color = c;
			}

			if (dt > DESTROY_TIME)
			{
				Destroy(gameObject);
			}
		}
	}

	void OnDestroy()
	{
		if (!GameManager.Instance.isApplicationQuitting && !Application.isLoadingLevel)
		{
			GameManager.Instance.UnregisterActor(this);
		}
	}

	public void Kill()
	{
		if (!m_killed)
		{
			m_killed = true;
			killedTime = Time.time;
		}
	}
}
