using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public int PlayerNum;

	public static float FIRE_COOLDOWN = 2.0f;

	private float lastFireTime = float.MinValue;

	private Actor _actor;
	private Actor m_actor {
		get {
			if (_actor == null) _actor = GetComponent<Actor>();
			return _actor;
		}
	}
	
	private float inputHorizontal
	{
		get {
			float value = Input.GetAxis ("Horizontal" + PlayerNum);
			return value == 0 ? 0 : Mathf.Sign(value);
		}
	}
	private float inputVertical
	{
		get {
			float value = Input.GetAxis ("Vertical" + PlayerNum);
			return value == 0 ? 0 : Mathf.Sign(value);
		}
	}
	
	private bool fire
	{
		get {
			return Input.GetButtonDown ("Fire" + PlayerNum);
		}
	}

	private bool pickUp
	{
		get {
			return false;//Input.GetKeyDown ("ButtonTop" + PlayerNum);
		}
	}

	public bool CanFire
	{
		get {
			return Time.time - lastFireTime > FIRE_COOLDOWN;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (m_actor.Killed) return;

		transform.localPosition = new Vector3(
			Mathf.Clamp(transform.localPosition.x + inputHorizontal * m_actor.Speed, -8.4f, 8.4f),
			Mathf.Clamp(transform.localPosition.y + inputVertical * m_actor.Speed, -4.5f, 4.5f),
			0
			);

		if (CanFire && fire)
		{
			lastFireTime = Time.time;
			KillClosestActor();
		}

		if (pickUp)
		{

		}
	}

	private bool KillClosestActor()
	{
		bool killedActor = false;
		foreach (Actor a in GameManager.Instance.GetActors().ToArray()) // To Array to duplicate list
		{
			if (a == m_actor) continue;

			Vector3 delta = transform.position - a.transform.position;
			if (delta.sqrMagnitude <= m_actor.AttackRange * m_actor.AttackRange)
			{
				if (!a.Killed)
				{
					a.Kill();
					GameManager.Instance.Scores[PlayerNum]++;
				}
				killedActor = true;
			}
		}

		return killedActor;
	}
}
