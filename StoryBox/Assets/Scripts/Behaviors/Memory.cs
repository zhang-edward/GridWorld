using System.Collections.Generic;
using System.Linq;

public class Memory {
	private Dictionary<string, object> values;

	public Memory() {
		values = new Dictionary<string, object>();
	}

	public object this[string s] {
		get => values.ContainsKey(s) ? values[s] : null;
		set {
			if (s != "")
				values[s] = value;
		}
	}

	public void SetDefault(string key, object value) {
		if (!values.ContainsKey(key)) {
			values[key] = value;
		}
	}

	/// <summary>
	/// Apply the target state on top of this one, overwriting duplicate keys with the new values
	/// </summary>
	/// <param name="state"></param>
	public void Apply(Memory other) {
		foreach (KeyValuePair<string, object> kvp in other.values)
			values[kvp.Key] = kvp.Value;
	}
}