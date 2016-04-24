using UnityEngine;
using System.Collections;
using System;

namespace Behaviors
{
	public class Sequence : Behavior {

		Behavior[] behaviors;
		int i = 0;					// current behavior that is running

		public Sequence(Behavior[] behaviors)
		{
			this.behaviors = behaviors;
		}

		public override void Reset()
		{
			i = 0;
			foreach (Behavior b in behaviors)
			{
				b.Reset();
			}
		}

		/// <summary>
		/// performs behavior
		/// </summary>
		/// <returns>behavior return code</returns>
		public override NodeStatus Act()
		{
			while (i < behaviors.Length)
			{
				switch (behaviors[i].Act())
				{
					case NodeStatus.Failure:
						//Debug.Log(behaviors[i] + ": Failure");
						return NodeStatus.Failure;
					case NodeStatus.Success:
						i++;
						//Debug.Log("success + continuing");
						continue;
					case NodeStatus.Running:
						//Debug.Log(behaviors[i] + ": Running");
						return NodeStatus.Running;
				}
			}
			// done with all behaviors and all success
			i = 0;
			return NodeStatus.Success;
		}
	}
}

