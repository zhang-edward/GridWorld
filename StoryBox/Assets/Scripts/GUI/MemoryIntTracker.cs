using UnityEngine;
using System.Collections;
using TMPro;

public class MemoryIntTracker: MonoBehaviour {

	public TMP_Text text;
	private Memory memory;
	private string key;

	public void Init(Memory memory, string key) {
		this.memory = memory;
		this.key = key;
	}

	public void Tick() {
		if (memory[key] != null)
			text.text = $"{key}: {(int)memory[key]}";
	}
}
