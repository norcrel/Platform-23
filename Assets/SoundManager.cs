using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public static SoundManager Instance;
	// Use this for initialization
	void Awake () {
		Instance = this;
	}

	public void PlaySound(string soundName)
	{
		AudioSource[] children = GetComponentsInChildren<AudioSource>();
		for (int i=children.Length-1; i >= 0; i--)
		{
			AudioSource child = children[i];
			if (child.name == soundName)
			{
				child.Play();
			}
		}
	}
}
