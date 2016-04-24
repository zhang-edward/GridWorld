using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Behaviors;

public class Hero : LivingEntity, IControllable
{
	// TWO MAIN STATS: Good vs Evil, Life vs Death
	// Negative values = evil/death
	/*private int Good = 0;
    private int Life = 0;*/

	public Blackboard blackboard;
	public Behavior behavior;

    private bool controlled;

    // TODO: make a Wallet class that contains helper methods for money managing
    public int money;
    public Quest currentQuest;

	void Awake()
	{
		blackboard = new Blackboard();
		behavior = new Selector(new Behavior[]
		{
			new Sequence(new Behavior[]
			{
				new Inverter(new CheckNull("Quest", blackboard)),
				new DoQuest(blackboard)
			}),

			new Sequence(new Behavior[]
			{
				new CheckNull("Quest", blackboard),
				new FindEntity<Town>(this, blackboard),
				new PathToEntity(this, blackboard),
				new GetQuest(this)
			}),
		});
	}

    public override void Act()
    {
		//Debug.Log("act");
		if (controlled)
		{
			// set default commands
			defaultCommands[0] = new MoveCommand(this, x, y + 1);
			defaultCommands[1] = new MoveCommand(this, x + 1, y);
			defaultCommands[2] = new MoveCommand(this, x, y - 1);
			defaultCommands[3] = new MoveCommand(this, x - 1, y);
			// initialize special interactions if entities are nearby
			InitCommands();
		}
		else
		{
			blackboard.setValue("Quest", currentQuest);
			blackboard.setValue("Money", money);
			defs.SetBlackboardValues(blackboard);
			NodeStatus s = behavior.Act();
			//Debug.Log("================Begin====================");
			//Debug.Log(s);
			if (s == NodeStatus.Success || s == NodeStatus.Failure)
				behavior.Reset();
			done = true;
			//Debug.Log("=================End=====================");
		}

		if (currentQuest != null)
		{
			currentQuest.checkIfCompleted();
			if (currentQuest.isCompleted())
				currentQuest = null;
		}
    }

    /// <summary>
    /// Create possible Commands for this entity to perform
    /// </summary>
    public void InitCommands()
    {
        Entity[] neighbors = world.getAdjacentEntities(x, y);
        for (int i = 0; i < 4; i++)
        {
			// reset the command at this position
			interactions[i] = null;

			// find the entity at the position
            Entity e = neighbors[i];
            if (e != null)
            {
                // ENEMY INTERACTION
                if (e is Pawn)
                    interactions[i] = new BattleCommand(this, e as Pawn);
				// TOWN INTERACTION
                else if (e is Town)
                {
                    // check if town has a quest
                    Town town = e as Town;
                    if (currentQuest == null && town.quest != null && town.quest.hero == null)
                        interactions[i] = new AcceptQuestCommand(this, e as Town);
                    else
                        Debug.Log(town + " has no more quests available");
                }
				// HERO INTERACTION
				else if (e is Hero)
				{
					Hero hero = e as Hero;
					if (hero.faction != this.faction)
						interactions[i] = new BattleCommand(this, e as Hero);
				}
            }
            else
            {
                interactions[i] = null;
            }
        }
    }

    public void GetQuest(Quest quest)
    {
		if (quest == null)
		Debug.Log(quest);
        quest.hero = this;
        currentQuest = quest;
    }

    public bool Controllable()
    {
		return true;//Alive();
    }

    public void InitControl()
    {
        controlled = true;
	}

    public void ReleaseControl()
    {
        controlled = false;
        done = true;
    }
}