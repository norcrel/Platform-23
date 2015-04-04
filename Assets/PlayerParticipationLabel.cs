using UnityEngine;
using UnityEngine.UI;

public class PlayerParticipationLabel : MonoBehaviour {
	public int PlayerNum;

	private Text m_text;
	private Color m_originalColor;

	void Start()
	{
		m_text = GetComponent<Text>();
		m_originalColor = m_text.color;
	}

	void Update()
	{
		m_text.enabled = GameManager.Instance.participatingPlayers[PlayerNum];

		if (m_text.enabled)
		{
			Player p = GameManager.Instance.GetPlayer(PlayerNum);
			if (p != null)
			{
				m_text.color = p.CanFire ? m_originalColor : Color.gray;
			}
		}
	}
}
