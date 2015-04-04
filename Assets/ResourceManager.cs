using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour {
	public static ResourceManager Instance;

	public Sprite[][] ActorSprites;
	public static string[] ACTOR_SPRITE_LIST = {
		"cloak",
		"martialartist",
		"mary",
		"pit",
		"potter",
		"purplegirl",
		"redsprite",
		"schoolgirl",
		"zombie"
	};

	public List<string> LimitedCostumeList = new List<string>();

	void Awake () {
		Instance = this;

		ActorSprites = new Sprite[ACTOR_SPRITE_LIST.Length][];
		for (int i=0; i < ACTOR_SPRITE_LIST.Length; i++)
		{
			ActorSprites[i] = Resources.LoadAll<Sprite>("characters/"+ACTOR_SPRITE_LIST[i]);
		}

		ResourceManager.Instance.GenerateLimitedCostumeList(ACTOR_SPRITE_LIST.Length);
	}

	public void GenerateLimitedCostumeList(int numCostumes)
	{
		// Build costume list
		LimitedCostumeList.Clear();
		while (LimitedCostumeList.Count < numCostumes)
		{
			string costumeName = ACTOR_SPRITE_LIST[UnityEngine.Random.Range(0, ACTOR_SPRITE_LIST.Length)];
			if (!LimitedCostumeList.Contains(costumeName))
			{
				LimitedCostumeList.Add(costumeName);
			}
		}
	}

	public string GetRandomCostumeFromLimitedList()
	{
		return LimitedCostumeList[UnityEngine.Random.Range (0, LimitedCostumeList.Count)];
	}
}
