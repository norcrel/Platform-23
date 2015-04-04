using UnityEngine;
using System.Collections;
using System;

public class AI : MonoBehaviour {
	private Action currentAction = null;

	private Actor _actor;
	private Actor m_actor {
		get
		{
			if (_actor == null) _actor = GetComponent<Actor>();
			return _actor;
		}
	}

	// Use this for initialization
	void Start () {
		if (currentAction == null)
		{
			currentAction = Idle;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (m_actor.Killed) return;

		currentAction();
	}

	public void SetTargetPosition(Vector3 target)
	{
		targetPosition = target;
		targetPosition.x = Mathf.Clamp(targetPosition.x, -7.68f, 7.68f);
		targetPosition.y = Mathf.Clamp(targetPosition.y, -4.5f, 4.5f);
		currentAction = Wander;
	}

	private Vector3 targetPosition = new Vector3();
	void Wander() {
		Vector3 delta = targetPosition - transform.position;
		float timeSpeed = m_actor.Speed * Time.deltaTime;
		if (delta.sqrMagnitude > timeSpeed*timeSpeed)
		{
			delta.Normalize();
			delta *= timeSpeed;
		}
		else
		{
			idleTime = 5f-Mathf.Sqrt(UnityEngine.Random.Range(0f, 25f));
			currentAction = Idle;
		}

		transform.position = delta + transform.position;
	}

	private float idleTime = 0;
	void Idle() {
		idleTime -= Time.deltaTime;

		if (idleTime <= 0)
		{
			float rand = UnityEngine.Random.value;
			if (rand > .6)
			{
				// Move Diagonal
				float deltaRand = UnityEngine.Random.Range (-4f, 4f);
				targetPosition.Set(
					Mathf.Clamp(transform.localPosition.x + deltaRand, -7.68f, 7.68f),
					Mathf.Clamp(transform.localPosition.y + deltaRand, -4.5f, 4.5f),
					0
					);
			}
			else if (rand > .3)
			{
				// Move Horizontal
				targetPosition.Set(
					UnityEngine.Random.Range (-7.68f, 7.68f),
					transform.localPosition.y,
					0
					);
			}
			else
			{
				// Move Vertical
				targetPosition.Set(
					transform.localPosition.x,
					UnityEngine.Random.Range (-4.5f, 4.5f),
					0
					);
			}
			currentAction = Wander;
		}
	}
}
