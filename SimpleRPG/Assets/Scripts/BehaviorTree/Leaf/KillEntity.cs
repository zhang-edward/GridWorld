using UnityEngine;
using System.Collections;

namespace Behaviors
{
	public class KillEntity : Behavior
	{
		private LivingEntity e;
		private LivingEntity target;

		public KillEntity(LivingEntity e, LivingEntity target)
		{
			this.e = e;
			this.target = target;
		}

		public override void Reset()
		{
		}

		public override NodeStatus Act()
		{
			Entity[] neighbors = e.world.getAdjacentEntities(e.x, e.y);
			foreach (Entity neighbor in neighbors)
			{
				if (neighbor == target)
				{
					BattleCommand command = new BattleCommand(e, target);
					command.Execute();
					if (!target.Alive())
					{
						return NodeStatus.Success;
					}
					else
					{
						return NodeStatus.Running;
					}
				}
			}
			return NodeStatus.Failure;
		}

	}
}

