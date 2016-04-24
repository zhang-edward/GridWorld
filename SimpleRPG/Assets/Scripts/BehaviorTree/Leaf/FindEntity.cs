using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Behaviors
{
	public class FindEntity<T> : Behavior
		where T : Entity
	{
		private Entity target;
		private Entity e;
		private Blackboard b;

		public FindEntity(Entity e, Blackboard b)
		{
			this.e = e;
			this.b = b;
		}

		public override void Reset()
		{
			b.setValue("TargetEntity", null);
		}

		public override NodeStatus Act()
		{
			target = e.world.findEntity<T>();
			if (target != null)
			{
				b.setValue("TargetEntity", target);
				//Debug.Log("Target: " + target + "\nTargetEntity: " + b.getValue("TargetEntity"));
				return NodeStatus.Success;
			}
			return NodeStatus.Failure;
		}
	}
}
