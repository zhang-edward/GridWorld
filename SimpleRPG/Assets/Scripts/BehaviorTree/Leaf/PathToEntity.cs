using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Behaviors
{
	public class PathToEntity : Behavior
	{
		private Entity e;
		private Blackboard b;

		private Entity target;

		private FindPath findPath;
		private MoveOnPath moveOnPath;

		List<Vector2> path = new List<Vector2>();
		private Vector2 empty = new Vector2(-1, -1);

		public PathToEntity(Entity e, Blackboard b)
		{
			this.e = e;
			this.b = b;
			findPath = new FindPath(e, b);
			moveOnPath = new MoveOnPath(e, b);
		}

		public override void Reset()
		{
			//Debug.Log("++++++++++++++++++ RESET ++++++++++++++++");
			b.setValue("TargetX", null);
			b.setValue("TargetY", null);
			b.setValue("Path", null);
			target = null;
		}

		public override NodeStatus Act()
		{
			if (target == null)
			{
				target = b.getValue("TargetEntity") as Entity;
				//Debug.Log(target);
			}
			// check if target is adjacent
			Entity[] neighbors = e.world.getAdjacentEntities(e.x, e.y);
			foreach (Entity neighbor in neighbors)
			{
				if (neighbor == target)
					return NodeStatus.Success;
			}

			if (b.getValue("Path") == null)
			{
				//Debug.Log("setting new targetx and y");
				List<Vector2> adjacentCoords = target.world.getAdjacentCoords(target.x, target.y);
				Vector2 randAdjacentCoord = adjacentCoords[Random.Range(0, adjacentCoords.Count)];

				b.setValue("TargetX", (int)randAdjacentCoord.x);
				b.setValue("TargetY", (int)randAdjacentCoord.y);
				switch (findPath.Act())
				{
					case NodeStatus.Success:
						break;
					case NodeStatus.Failure:
						return NodeStatus.Failure;
					default:
						return NodeStatus.Failure;
				}
			}

			switch (moveOnPath.Act())
			{
				case NodeStatus.Running:
					// get new path if we have covered over half of the path
					int pathLeft = (int)b.getValue("PathLeft");
					int pathCount = (int)b.getValue("PathLength");
					if (pathLeft <= (pathCount / 2))
						b.setValue("Path", null);
					return NodeStatus.Running;
			}
			//Debug.Log("idk");
			return NodeStatus.Failure;
		}
	}
}

