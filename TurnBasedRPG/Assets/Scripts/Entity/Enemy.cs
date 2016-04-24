using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Entity {

	public HeroEntity target;
	public EnemyStats stats;

	public PlayerEffector damager;

	public override void Init(Level level)
	{
		base.Init(level);
		LevelManager.instance.ITObjects.Add (damager);
	}

	public void InitStats(EnemyStats stats)
	{
		this.stats = stats;
	}

	public override void Act()
	{
		if (target == null || target.stats.health <= 0)
			FindTarget();

		if (level.getAdjacentEntities(pos).Contains(target))
			AttackPlayer();
		else
			MoveTowardsPlayer();
//		StartCoroutine("WaitForITObjects");
		done = true;
	}

	/*public IEnumerator WaitForITObjects()
	{
		bool ITObjectsDone = false;
		while (!ITObjectsDone)
		{
			ITObjectsDone = true;
			foreach (IntraTurnObj itObj in ITObjects)
			{
				if (!itObj.done)
					ITObjectsDone = false;
			}
			yield return null;
		}

		foreach (IntraTurnObj itObj in ITObjects)
			itObj.Deactivate();
		done = true;
	}*/

	protected void FindTarget()
	{
		int minDistance = int.MaxValue;
		foreach (Entity e in LevelManager.instance.playerList)
		{
			int newDistance = Coords.Distance(e.pos, this.pos);
			if (newDistance < minDistance)
			{
				minDistance = newDistance;
				target = e as HeroEntity;
			}
		}
	}

	private void MoveTowardsPlayer()
	{
		// to randomize movements
		List<Coords> destList = new List<Coords>();
		if (pos.x < target.pos.x)
		{
			destList.Add(new Coords(pos.x + 1, pos.y));
		}
		else if (pos.x > target.pos.x)
		{
			destList.Add(new Coords(pos.x - 1, pos.y));
		}
		if (pos.y < target.pos.y)
		{
			destList.Add(new Coords(pos.x, pos.y + 1));
		}
		else if (pos.y > target.pos.y)
		{
			destList.Add(new Coords(pos.x, pos.y - 1));
		}
		int randIndex = Random.Range(0, destList.Count);
		Move(destList[randIndex]);
	}

	private void AttackPlayer()
	{
		Debug.Log("Attacking player");
		damager.Init(target, stats.damage, target.pos);
		damager.Activate();
	}
}