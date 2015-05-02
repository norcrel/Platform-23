using UnityEngine;
using System.Collections;

public class Sweeper : MonoBehaviour {
	public GameObject SweepObject;

	public float StartDelay = 3f; // Initial delay time in seconds
	public float SweepSpeed = 500f; // Degrees per second
	public float SweepTime = 3f; // Sweep time in seconds
	public float FadeInOutTime = 1f; // Time taken to fade in/out the sweeper

	private static readonly float REV_UP_TIME = 5f;

	private float m_totalTimeSweeped = 0f;

	void Awake()
	{
		SweepObject.SetActive(false);
		if (renderer != null)
		{
			renderer.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (StartDelay > 0)
		{
			StartDelay -= Time.deltaTime;
			return;
		}

		Actor a = GetComponent<Actor>();
		if (a != null && a.Killed)
		{
			SweepObject.SetActive(false);
			return;
		}

		m_totalTimeSweeped += Time.deltaTime;

		if (SweepTime > 0)
		{
			float revUpScale = Mathf.Min (Mathf.Min(m_totalTimeSweeped / REV_UP_TIME, SweepTime / REV_UP_TIME), 1);
			SweepObject.SetActive(true);

			Vector3 scale = SweepObject.transform.localScale;
			scale.y = revUpScale * 8;
			SweepObject.transform.localScale = scale;

			float angle = SweepObject.transform.localEulerAngles.z - SweepSpeed*Time.deltaTime;
			SweepObject.transform.localEulerAngles = new Vector3(0f, 0f, angle);

			for (int i=0; i < GameManager.Instance.allPlayers.Length; i++)
			{
				Player p = GameManager.Instance.allPlayers[i];
				if (p != null)
				{
					Vector3 delta = p.transform.position - transform.position;
					if (delta.sqrMagnitude < revUpScale * 6 &&
					    Vector3.Angle(Quaternion.Euler (0, 0, SweepObject.transform.eulerAngles.z) * Vector3.up, delta) < 5)
					{
						p.TempReveal();

						SoundManager.Instance.PlaySound("alarmSFX");
					}
				}
			}

			SweepTime -= Time.deltaTime;
		}

		if (SweepTime < 0)
		{
			Destroy (gameObject);
		}
	}
}
