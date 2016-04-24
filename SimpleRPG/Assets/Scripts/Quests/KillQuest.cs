using UnityEngine;
using System.Collections;
using System;
using Behaviors;

public class KillQuest : Quest{

    private ICombatable target;
    
    public KillQuest(Town t, ICombatable target)
    {
        town = t;
        this.target = target;
        Debug.Log("New " + this + " by " + t + " with target " + target);
    }

    public override void checkIfCompleted()
    {
        if (!target.Alive())
        {
            completeQuest();
        }
    }

	/// <summary>
	/// Get the behaviorTree that will do the correct actions to complete this quest
	/// </summary>
	/// <param name="e">The entity doing the quest</param>
	/// <returns></returns>
	public override Behavior getAiCompletionTree()
	{
		Behavior aiCompletion = new Sequence(new Behavior[]
		{
			new SetBlackboardValue(hero.blackboard, "TargetEntity", target),
			new PathToEntity(hero, hero.blackboard),
			new KillEntity(hero, target as LivingEntity)
		});
		return aiCompletion;
	}
}
