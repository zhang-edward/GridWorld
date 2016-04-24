using UnityEngine;
using System.Collections;
using Behaviors;

public class EntityDefs {

	public int maxHealth = 10;
	public int health = 9;

	public int experience;
	public int Level
	{
		get { return experience / 100; }
		set { experience = value * 100; }
	}

	public EntityDefs()
	{
	}

	public void SetBlackboardValues(Blackboard b)
	{
		b.setValue("Level", Level);
		b.setValue("Experience", experience);
		b.setValue("Health", health);
		b.setValue("MaxHealth", maxHealth);
	}
}
