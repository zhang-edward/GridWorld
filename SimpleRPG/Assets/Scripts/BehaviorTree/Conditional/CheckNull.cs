using UnityEngine;
using System.Collections;

namespace Behaviors
{
	public class CheckNull : Behavior {

		private string key;
		private Blackboard b;

		public CheckNull(string key, Blackboard b)
		{
			this.key = key;
			this.b = b;
		}

		public override void Reset()
		{
		}

		public override NodeStatus Act()
		{
			if (b.getValue(key) == null)
			{
				return NodeStatus.Success;
			}
			return NodeStatus.Failure;
		}
	}
}