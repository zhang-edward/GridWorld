using System.Collections.Generic;

public class Memory {
	private Dictionary<string, object> values;

	public Memory() {
		values = new Dictionary<string, object>();
	}
	
	public object this[string s] {
		get => values[s];
		set => values[s] = value;
	}
}