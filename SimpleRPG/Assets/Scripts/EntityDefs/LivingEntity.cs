using UnityEngine;
using System.Collections;
using System;

public abstract class LivingEntity : Entity, ICombatable
{
	public EntityDefs defs = new EntityDefs();
	public CombatDefs combatDefs = new CombatDefs();
	public int faction;

	public Command[] defaultCommands = new Command[4];       // commands used if no command was explicitly set
	public Command[] interactions = new Command[4];

	public void ExecuteCommand(int pos)
	{
		// Execute the command for that position
		if (pos != -1)
		{
			if (interactions[pos] != null)
				interactions[pos].Execute();
			else
				defaultCommands[pos].Execute();
		}
	}

	public bool Damage(int amt)
	{
		defs.health -= amt;
		if (defs.health <= 0)
		{
			// FOR DEBUGGING
			DisableSelf();
			return true;
		}
		return false;
	}

	public void Heal(int amt)
	{
		defs.health += amt;
	}

	public bool Alive()
	{
		return defs.health > 0;
	}

	public void AddExperience(int amt)
	{
		defs.experience += amt;
	}
}
