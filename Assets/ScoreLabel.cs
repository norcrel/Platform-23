using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreLabel : MonoBehaviour {
	public int PlayerNum;

	private Text _textMesh;
	private Text m_textMesh {
		get {
			if (_textMesh == null) _textMesh = GetComponent<Text>();
			return _textMesh;
		}
	}

	private Player _player;
	private Player m_player {
		get {
			if (_player == null) _player = GameManager.Instance.GetPlayer (PlayerNum);
			return _player;
		}
	}

	private Color originalColor;

	void Awake()
	{
		originalColor = m_textMesh.color;
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerNum % 2 == 0)
		{
			m_textMesh.text = "P" + (PlayerNum+1) + " - " + GameManager.Instance.Scores[PlayerNum];
		}
		else
		{
			m_textMesh.text = GameManager.Instance.Scores[PlayerNum] + " - P" + (PlayerNum+1);
		}

		if (m_player == null)
		{
			m_textMesh.fontSize = 20;
			m_textMesh.color = new Color(.5f, .5f, .5f);
		}
		else
		{
			if (!m_player.CanFire)
			{
				m_textMesh.color = new Color(.5f, .5f, .5f);
			}
			else
			{
				m_textMesh.color = originalColor;
			}
		}
	}
}
