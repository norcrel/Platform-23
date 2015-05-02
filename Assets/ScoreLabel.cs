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

	private GameObject[] m_scoreIcons = new GameObject[3];

	private Color originalColor;

	void Awake()
	{
		originalColor = m_textMesh.color;
	}
	
	// Update is called once per frame
	void Update () {
		string descriptionText = "";

		if (m_player == null)
		{
			m_textMesh.fontSize = 20;
			m_textMesh.color = new Color(.5f, .5f, .5f);
			descriptionText += "DEAD";
		}
		else
		{
			if (!m_player.CanFire)
			{
				m_textMesh.color = new Color(.5f, .5f, .5f);
				descriptionText += "Reloading...";
			}
			else
			{
				m_textMesh.color = originalColor;
			}
		}


		if (!string.IsNullOrEmpty(descriptionText))
		{
			if (PlayerNum % 2 == 0)
			{
				descriptionText = "P" + (PlayerNum+1) + " - " + descriptionText;
			}
			else
			{
				descriptionText = descriptionText + " - P" + (PlayerNum+1);
			}
		}
		else
		{
			descriptionText = "P" + (PlayerNum+1);
		}

		m_textMesh.text = descriptionText;

		// Layout scores
		if (m_player != null)
		{

			int currentScore = GameManager.Instance.TreasureScores[m_player.PlayerNum];
			int i;
			for (i=0; i < currentScore; i++)
			{
				if (m_scoreIcons[i] == null)
				{
					GameObject iconObject = new GameObject();
					iconObject.transform.parent = transform;
					SpriteRenderer sr = iconObject.AddComponent<SpriteRenderer>();
					sr.sprite = Resources.Load<Sprite>("briefcase");
					iconObject.transform.localScale = new Vector3(5, 5, 1);
					iconObject.transform.localPosition = new Vector3(10*i,
					                                                 m_player.PlayerNum < 2 ? -60 - 10*i : 10 + 10*i,
					                                                 0);

					m_scoreIcons[i] = iconObject;
				}
			}

			// Destroy extra ones
			for (; i < m_scoreIcons.Length; i++)
			{
				if (m_scoreIcons[i] != null)
				{
					Destroy (m_scoreIcons[i]);
				}
			}
		}

	}
}
