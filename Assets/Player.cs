using UnityEngine;
using System;

public class Player : MonoBehaviour {
	public int PlayerNum;

	public static float FIRE_COOLDOWN = 3.0f;

	private float lastFireTime = float.MinValue;
	private float m_revealTime = 0;

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
			if (value != 0)
			{
				return Mathf.Sign (value);
			}
			float dValue = Input.GetAxis ("DHorizontal" + PlayerNum);
			if (dValue != 0)
			{
				return Mathf.Sign (dValue);
			}
			return 0;
		}
	}
	private float inputVertical
	{
		get {
			float value = Input.GetAxis ("Vertical" + PlayerNum);
			if (value != 0)
			{
				return Mathf.Sign (value);
			}
			float dValue = Input.GetAxis ("DVertical" + PlayerNum);
			if (dValue != 0)
			{
				return Mathf.Sign (dValue);
			}
			return 0;
		}
	}
	
	private bool fire
	{
		get {
			return Input.GetButtonDown ("RightButton" + PlayerNum);
		}
	}
	
	private bool attack
	{
		get {
			return Input.GetButtonDown ("LeftButton" + PlayerNum);
		}
	}
	
	private bool change
	{
		get {
			return Input.GetButtonDown ("TopButton" + PlayerNum);
		}
	}
	
	private bool run
	{
		get {
			return Input.GetButton ("BottomButton" + PlayerNum);
		}
	}
	
	private bool start
	{
		get {
			return Input.GetButton ("Start" + PlayerNum) || Input.GetButton ("Logo" + PlayerNum) || Input.GetButton ("Pad" + PlayerNum);
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

		float normalizedSpeed = m_actor.Speed * Time.deltaTime;
		if (run)
		{
			if (CanFire)
			{
				normalizedSpeed *= 2f;
			}
			else
			{
				normalizedSpeed *= 1.2f;
			}
		}

		transform.localPosition = new Vector3(
			Mathf.Clamp(transform.localPosition.x + inputHorizontal * normalizedSpeed, -7.68f, 7.68f),
			Mathf.Clamp(transform.localPosition.y + inputVertical * normalizedSpeed, -4.5f, 4.5f),
			0
			);

		if (CanFire && attack)
		{
			lastFireTime = Time.time;
			if (KillClosestActor())
			{
				SoundManager.Instance.PlaySound("killSFX");
			}

			SoundManager.Instance.PlaySound("attackSFX");
		}

		if (change)
		{
			m_actor.ChangeToRandomCostume();
		}

		if (m_revealTime > 0)
		{
			m_revealTime -= Time.deltaTime;
			SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
			if (m_revealTime < 0)
			{
				sr.color = Color.white;
			}
			else
			{	
				sr.color = new Color((float) (Math.Sin(Time.realtimeSinceStartup*6f)/2f + .5f), 0, 0);
			}
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

	public void TempReveal()
	{
		m_revealTime = 2.5f;
	}
}
