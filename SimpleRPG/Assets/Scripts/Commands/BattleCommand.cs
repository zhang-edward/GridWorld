using UnityEngine;
using System.Collections;

public class BattleCommand : Command
{
	public new LivingEntity e;
    public LivingEntity target;

    /// <summary>
    /// Attack a target
    /// </summary>
    /// <param name="e">The entity performing the action</param>
    /// <param name="target">The target of the attack</param>
    /// <param name="damage">Damage to deal to the target</param>
    public BattleCommand(LivingEntity e, LivingEntity target)
    {
        this.e = e;
        this.target = target;
    }

    public override void Execute()
    {
		Vector2 targetPos = World.WorldCoord2Position(target.x, target.y);
		e.anim.Animate("Battle", targetPos);

		target.Damage(calculateDamage());
		e.AddExperience(target.combatDefs.experienceRewardOnAttack());
		Debug.Log(e + " started battle with " + target);

        e.done = true;
    }

	private int calculateDamage()
	{
		return e.combatDefs.strength - target.combatDefs.defense;
	}

    public override string ToString()
    {
        return base.ToString();
    }
}
