using UnityEngine;
using System.Collections;

public class Treasure : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		for (int i=0; i < GameManager.Instance.allPlayers.Length; i++)
		{
			Player p = GameManager.Instance.allPlayers[i];
			if (p != null)
			{
				Vector3 delta = p.transform.position - transform.position;
				if (delta.sqrMagnitude < .2)
				{
					GameManager.Instance.ScoreTreasure(p);
					SoundManager.Instance.PlaySound("treasureSFX");
					Destroy (gameObject);
					break;
				}
			}
		}
	}
}
