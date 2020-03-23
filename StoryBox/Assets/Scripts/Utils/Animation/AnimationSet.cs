using UnityEngine;

[System.Serializable]
public class AnimationSet
{
	public AnimationSetDictionary dict;

	// Indexer
	public SimpleAnimation this[string s]
	{ get { return dict.ContainsKey(s) ? dict[s] : null; } }
}
