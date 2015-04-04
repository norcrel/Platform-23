using UnityEngine;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class Actor : MonoBehaviour {

	public float Speed = 1.5f; // Movement speed

	public float AttackRange = 1f; // Attack and interaction range

	private bool m_killed = false;
	private float killedTime;
	private const float DESTROY_TIME = 3.0f;
	public bool Killed { get { return m_killed; } }
	
	public Action OnActorKilled;

	private float currentFrame = 0;
	private int currentHeading = 0;
	public string CostumeName = "";

	private Vector3 lastPosition;

	// Use this for initialization
	void Start () {
		GameManager.Instance.RegisterActor(this);

		if (string.IsNullOrEmpty(CostumeName))
		{
			CostumeName = ResourceManager.ACTOR_SPRITE_LIST[UnityEngine.Random.Range(0, ResourceManager.ACTOR_SPRITE_LIST.Length)];
		}

		lastPosition = transform.position;

		FaceRandom();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_killed)
		{
			float dt = Time.time - killedTime;
			if (dt > 1.0)
			{
				Color c = renderer.material.color;
				c.a = 1 - dt / DESTROY_TIME;
				renderer.material.color = c;
			}

			if (dt > DESTROY_TIME)
			{
				Destroy(gameObject);
			}
		}

		if (lastPosition != transform.localPosition)
		{
			Vector3 delta = transform.localPosition - lastPosition;

			if (delta.x > 0)
			{
				FaceLeft();
			}
			else if (delta.x < 0)
			{
				FaceRight();
			}
			
			if (delta.y > 0)
			{
				FaceUp();
			}
			else if (delta.y < 0)
			{
				FaceDown();
			}

			renderer.sortingOrder = (int) (-transform.position.y * 100f);

			currentFrame = (currentFrame+Time.deltaTime*8f*(Speed/1.5f))%4;
			while (currentFrame >= 4) currentFrame -= 4f;
			refreshFrame();
		}
		lastPosition = transform.localPosition;
	}

	public void ChangeToRandomCostume()
	{
		string prevCostume = CostumeName;
		while (prevCostume == CostumeName)
		{
			CostumeName = ResourceManager.Instance.GetRandomCostumeFromLimitedList();
		}

		refreshFrame();
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

			if (OnActorKilled != null) OnActorKilled();
		}
	}

	public void FaceLeft()
	{
		currentHeading = 8;
		refreshFrame();
	}

	public void FaceRight()
	{
		currentHeading = 4;
		refreshFrame();
	}

	public void FaceUp()
	{
		currentHeading = 12;
		refreshFrame();
	}

	public void FaceDown()
	{
		currentHeading = 0;
		refreshFrame();
	}

	public void FaceRandom()
	{
		currentHeading = UnityEngine.Random.Range (0, 4) * 4;
		refreshFrame();
	}

	void OnTriggerEnter2D(Collider2D c)
	{
	}

	private void refreshFrame()
	{
		loadFrame ((int)(currentHeading + currentFrame));
	}

	private void loadFrame(int frameNum)
	{
		((SpriteRenderer) renderer).sprite = Resources.LoadAll<Sprite>("characters/"+CostumeName)[frameNum];
	}
}
