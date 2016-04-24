using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Behaviors
{
	public class Blackboard {

		public Dictionary<string, object> values = new Dictionary<string, object>();
		public static Dictionary<string, object> globalValues = new Dictionary<string, object>();

		public object getValue(string key)
		{
			object answer;
			if (values.TryGetValue(key, out answer))
				return answer;
			return null;
		}

		public void setValue(string key, object value)
		{
			values[key] = value;
		}
	}
}
