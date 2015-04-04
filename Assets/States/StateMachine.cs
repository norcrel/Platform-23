using UnityEngine;
using System.Collections;

public class StateMachine : MonoBehaviour {
	private State m_currentState;
	public State CurrentState { get { return m_currentState; } }
	
	// Update is called once per frame
	void Update () {
		if (m_currentState != null)
		{
			m_currentState.Update();
		}
	}

	public void ChangeState(State s)
	{
		if (m_currentState != null)
		{
			m_currentState.Exit();
		}

		m_currentState = s;

		if (m_currentState != null)
		{
			m_currentState.Init(this);
			m_currentState.Enter();
		}
	}
}
