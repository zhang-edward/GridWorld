using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[System.Serializable]
public class BehaviorTreeDict : SerializableDictionaryBase<string, Behavior> {}