using UnityEngine;
using System.Collections;
using System;

namespace Behaviors
{
	public class Succeeder : Behavior
	{

		private Behavior behavior;

		public Succeeder(Behavior behavior)
		{
			this.behavior = behavior;
		}

		public override void Reset()
		{
			behavior.Reset();
		}

		public override NodeStatus Act()
		{
			behavior.Act();
			return NodeStatus.Success;
		}
	}
}

