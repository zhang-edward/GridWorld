using System;

[Serializable]
public class EffectorStats
{
	public int range;
	public int damage;
	public enum EffectorType
	{
		Point,
		Projectile
	}
	public EffectorType type;

	public float speed;
	public bool camShakeOnImpact;

	public int animID;

	public EffectorStats(int range, int damage, EffectorType type)
	{
		this.range = range;
		this.damage = damage;
		this.type = type;
	}
}