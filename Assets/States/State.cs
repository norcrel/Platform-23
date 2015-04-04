

public class State {
	protected StateMachine m_owner;

	public State() {}

	public void Init(StateMachine owner)
	{
		m_owner = owner;
	}

	virtual public void Enter() {}
	virtual public void Update() {}
	virtual public void Exit() {}
}
