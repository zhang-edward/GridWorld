using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {
	private static System.Random rng = new System.Random(); // used in Shuffle extension method

	public static bool ContainsParam(this Animator _Anim, string _ParamName) {
		foreach (AnimatorControllerParameter param in _Anim.parameters) {
			if (param.name == _ParamName) return true;
		}
		return false;
	}

	public static int ManhattanDistance(this Vector2Int vec, Vector2Int other) {
		return Mathf.Abs(vec.x - other.x) + Mathf.Abs(vec.y - other.y);
	}

	public static void Shuffle<T>(this IList<T> list) {
		int n = list.Count;
		while (n > 1) {
			n--;
			int k = rng.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}