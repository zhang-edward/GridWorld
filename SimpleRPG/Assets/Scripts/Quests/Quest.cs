using UnityEngine;
using System.Collections;
using Behaviors;

public abstract class Quest {

    public Town town;           // the town that issued the quest
    public Hero hero;           // the hero that is doing the quest
    public int moneyReward;     // amt of money rewarded upon completion
	public int expReward;       // the amt of exp rewarded upon completion

	private bool Completed;

    public abstract void checkIfCompleted();

	//public abstract void showTarget();

    public void setReward(int money, int exp)
    {
        moneyReward = money;
        expReward = exp;
    }

    public void completeQuest()
    {
        Debug.Log("Quest Complete");
        hero.money += moneyReward;
		hero.AddExperience(expReward);
		town.quest = null;
		Completed = true;
    }

	public bool isCompleted()
	{ return Completed; }

	public abstract Behavior getAiCompletionTree();
}
