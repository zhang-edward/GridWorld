using UnityEngine;
using System.Collections;

namespace Behaviors
{
	public class GetQuest : Behavior {

		private Hero hero;

		public GetQuest(Hero hero)
		{
			this.hero = hero;
		}

		public override void Reset()
		{ 
		}

		public override NodeStatus Act()
		{
			Entity[] neighbors = hero.world.getAdjacentEntities(hero.x, hero.y);
			foreach (Entity e in neighbors)
			{
				if (e is Town)
				{
					Town town = e as Town;
					if (town.quest != null && town.quest.hero == null)
					{
						AcceptQuestCommand command = new AcceptQuestCommand(hero, town);
						command.Execute();
						return NodeStatus.Success;
					}
				}
			}
			return NodeStatus.Failure;
		}

	}
}
