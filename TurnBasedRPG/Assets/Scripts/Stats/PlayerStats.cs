using UnityEngine;
using System.Collections;

[System.Serializable]
public class HeroStats {

	public int health;
	public int damage;
	public int moveRange;

	public BaseWeapon weapon;

	public int bodyID;

	public HeroStats()
	{
	}
		
	public static HeroStats DebugMakeNewPlayerStats()
	{
		HeroStats stats = new HeroStats ();
		stats.health = Random.Range (10, 20);
		stats.damage = Random.Range (3, 5);
		stats.moveRange = 2;
		stats.weapon = BaseWeapon.DebugMakeRandomWeapon ();
		return stats;
	}
}
