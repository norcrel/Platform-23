using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RulesDisplay : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		Text treasureRule = GetComponent<Text>();

		if (GameManager.Instance.PlayWithTreasure)
		{
			treasureRule.color = Color.white;
		}
		else
		{
			treasureRule.color = new Color(.4f, .4f, .4f);
		}
	}
}
