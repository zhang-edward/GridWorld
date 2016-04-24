using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace Behaviors
{
	public class SetBlackboardValue : Behavior
	{
		private Blackboard b;
		private string key;
		private object value;

		public SetBlackboardValue(Blackboard b, string key, object value)
		{
			this.b = b;
			this.key = key;
			this.value = value;
		}

		public override void Reset()
		{
		}

		public override NodeStatus Act()
		{
			//Debug.Log("setting blackboard value");
			b.setValue(key, value);
			return NodeStatus.Success;
		}
	}
}
