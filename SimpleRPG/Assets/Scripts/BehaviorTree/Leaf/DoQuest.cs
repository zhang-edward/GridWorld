using UnityEngine;
using System.Collections;
using System;

namespace Behaviors
{
	public class DoQuest : Behavior {

		private Blackboard b;
		private Quest quest;
		private Behavior behavior;

		public DoQuest(Blackboard b)
		{
			this.b = b;
		}

		public override void Reset()
		{
			quest = b.getValue("Quest") as Quest;
			if (quest != null)
			{
				behavior = quest.getAiCompletionTree();
				behavior.Reset();
			}
		}

		public override NodeStatus Act()
		{
			//Debug.Log("QUEST======================");
			if (quest == null)
			{
				return NodeStatus.Failure;
			}
			NodeStatus status = behavior.Act();
			if (quest.isCompleted())
			{
				Reset();
				return NodeStatus.Success;
			}
			else
			{
				return status;
			}

		}
	}
}