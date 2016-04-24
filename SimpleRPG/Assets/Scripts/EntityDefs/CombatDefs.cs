using UnityEngine;
using System.Collections;

public class CombatDefs
{
	public int strength = 1;
	public int defense = 0;

	public CombatDefs()
	{
	}

	public int experienceRewardOnAttack()
	{
		return 11;
	}

	public int moneyRewardOnAttack(float stealChance)
	{
		return 10;
	}

}
