using UnityEngine;
using System.Collections;

public class AcceptQuestCommand : Command
{
    public Town town;

    /// <summary>
    /// Accept a quest
    /// </summary>
    /// <param name="hero">The entity performing the action</param>
    /// <param name="town">The town issuing the quest</param>
    public AcceptQuestCommand(Entity e, Town town)
    {
        this.e = e;
        this.town = town;
    }

    public override void Execute()
    {
		e.anim.Animate("Quest");

        if (!(e is Hero))
            Debug.LogError("AcceptQuestCommand cannot be executed on " + e);
        else
        {
            Hero hero = e as Hero;
            hero.GetQuest(town.quest);
            Debug.Log(hero + " has accepted " + town.quest);
        }
        e.done = true;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
