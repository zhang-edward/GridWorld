using UnityEngine;
using System.Collections;
using System;

namespace Behaviors
{
	public class Inverter : Behavior
	{

		private Behavior behavior;

		public Inverter(Behavior behavior)
		{
			this.behavior = behavior;
		}

		public override void Reset()
		{
			behavior.Reset();
		}

		public override NodeStatus Act()
		{
			//Debug.Log("inverter");
			switch (behavior.Act())
			{
				case NodeStatus.Failure:
					return NodeStatus.Success;
				case NodeStatus.Success:
					return NodeStatus.Failure;
				case NodeStatus.Running:
					return NodeStatus.Running;
			}
			return NodeStatus.Failure;
		}
	}
}

