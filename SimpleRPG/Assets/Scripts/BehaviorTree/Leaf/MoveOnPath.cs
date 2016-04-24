using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Behaviors
{
	public class MoveOnPath : Behavior
	{
		private Entity e;
		private Blackboard b;
		private List<Vector2> path;

		public MoveOnPath(Entity e, Blackboard b)
		{
			this.e = e;
			this.b = b;
		}

		public override void Reset()
		{
		}

		public override NodeStatus Act()
		{
			path = b.getValue("Path") as List<Vector2>;
			if (path == null || path.Count <= 0)
			{
				Debug.Log("move on path: no path failed");
				return NodeStatus.Failure;
			}
			else if (!e.world.EntityExistsAt((int)path[0].x, (int)path[0].y))
			{
				MoveCommand command = new MoveCommand(e, (int)path[0].x, (int)path[0].y);
				path.RemoveAt(0);
				command.Execute();

				b.setValue("PathLeft", path.Count);
				// DEBUG ============================
				//Debug.Log("Length: " + b.getValue("PathLength"));
				//Debug.Log("Left: " + b.getValue("PathLeft"));
				/*if (path.Count <= 5)
					Debug.Log("Path: " + path.Count);*/
				// ==================================
				if (path.Count <= 0)
					return NodeStatus.Success;
				else
					return NodeStatus.Running;
			}

			//Debug.Log("move on path failed");
			return NodeStatus.Failure;
		}
	}
}
