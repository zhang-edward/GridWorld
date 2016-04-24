using UnityEngine;
using System.Collections;
using System;
using Behaviors;

public class TravelQuest : Quest
{
    private int targetX;
    private int targetY;

	public TravelQuest(Town t, int x, int y)
	{
		town = t;
		targetX = x;
		targetY = y;
		Debug.Log("New " + this + " by " + t + " with target " + targetX + ", " + targetY);
	}

    public override void checkIfCompleted()
    {
        if (hero.x == targetX && hero.y == targetY)
            completeQuest();
    }

	public override Behavior getAiCompletionTree()
	{
		return new Sequence(new Behavior[]
		{
			new SetBlackboardValue(hero.blackboard, "TargetX", targetX),
			new SetBlackboardValue(hero.blackboard, "TargetY", targetY),
			new FindPath(hero, hero.blackboard),
			new MoveOnPath(hero, hero.blackboard)
		});
	}
}
