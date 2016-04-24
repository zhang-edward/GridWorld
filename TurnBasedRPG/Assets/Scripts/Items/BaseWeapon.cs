using UnityEngine;

[System.Serializable]
public class BaseWeapon : BaseStatItem
{
	public int spriteID;
	public enum WeaponType {
		Staff,
		Wand,
		Orb
	}
	public WeaponType weaponType;
	public EffectorStats[] effectorStats;

	public BaseWeapon()
	{
		
	}

	public static BaseWeapon DebugMakeRandomWeapon()
	{
		BaseWeapon answer = new BaseWeapon();
		answer.weaponType = (WeaponType)Random.Range (0, 3);
		// TODO: make a static const int for max number of effectors
		answer.effectorStats = new EffectorStats[Random.Range (1, 3)];
		for (int i = 0; i < answer.effectorStats.Length; i ++)
		{
			int damage = Random.Range (1, 3);
			int range = Random.Range (3, 5);
			// TODO: figure out how to store the number of different effectortypes in a variable for this to work
			answer.effectorStats[i] = new EffectorStats (range, damage, (EffectorStats.EffectorType)Random.Range(0, 2));
		}
		return answer;
	}
}