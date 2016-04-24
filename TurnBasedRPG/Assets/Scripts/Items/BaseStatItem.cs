
[System.Serializable]
public class BaseStatItem : BaseItem
{
	public int health;
	public int damage;

	public BaseStatItem()
	{}

	public BaseStatItem(int health, int damage)
	{
		this.health = health;
		this.damage = damage;
	}
}