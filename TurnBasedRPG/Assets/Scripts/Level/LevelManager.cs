using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance;

	public Level level;

	public List<HeroEntity> playerList;
	public List<Entity> entitiesList;

	public GameObject playerEntityPrefab;
	public GameObject enemyEntityPrefab;

	public PlayerActionMenu actionMenu;
	public List<IntraTurnObj> ITObjects;

	public void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(this.gameObject);

		// DEBUG DEBUG DEBUG
		Game.playerData.Init ();
		// DEBUG DEBUG DEBUG
	}

	public void Start()
	{
		level.Init();
		level.GenerateLevel ();

		List<EnemyStats> dummyList = new List<EnemyStats>();
		dummyList.Add(new EnemyStats());
		dummyList.Add(new EnemyStats());
		CreateEntities(dummyList, enemyEntityPrefab);

		/*List<PlayerStats> dummyList2 = new List<PlayerStats>();
		dummyList2.Add(PlayerStats.DebugMakeNewPlayerStats());
		dummyList2.Add(PlayerStats.DebugMakeNewPlayerStats());*/
		CreatePlayers(Game.playerData.players, playerEntityPrefab);

		StartCoroutine("GameLoop");
	}

	private IEnumerator GameLoop()
	{
		while (true)
		{
			foreach (Entity p in playerList)
			{
				if (!p.done)
				{
					// initialize action menu
					actionMenu.Init (p as HeroEntity);
					// act
					p.Act();
					// wait until entity is done
					while (!p.done)
						yield return false;
					yield return new WaitUntil (ITObjectsDone);
					// check if the level is complete
					if (CheckLevelComplete())
					{
						ResetLevel ();
						break;
					}
				}
			}
			foreach (Entity e in entitiesList)
			{
				if (!e.done)
				{
					e.Act();
					while (!e.done)
						yield return false;
					yield return new WaitUntil (ITObjectsDone);
				}
			}

			//Debug.Log("All entities done");

			foreach (Entity e in playerList)
				e.Reset ();
			foreach (Entity e in entitiesList)
				e.Reset ();

			yield return null;
		}
	}

	public void ResetLevel()
	{
		StopAllCoroutines ();

		// TODO implement object pooling to avoid instantiate/destroy
		foreach (Entity e in playerList)
			Destroy (e.gameObject);
		foreach (Entity e in entitiesList)
			Destroy (e.gameObject);
		playerList.Clear ();
		entitiesList.Clear ();
		ITObjects.Clear ();
		level.Reset ();

		level.GenerateLevel();

		List<EnemyStats> dummyList = new List<EnemyStats>();
		dummyList.Add(new EnemyStats());
		dummyList.Add(new EnemyStats());
		CreateEntities(dummyList, enemyEntityPrefab);

		CreatePlayers(Game.playerData.players, playerEntityPrefab);


		StartCoroutine("GameLoop");
	}

	public bool ITObjectsDone()
	{
		foreach (IntraTurnObj itObj in ITObjects)
		{
			if (!itObj.done)
				return false;
		}

		foreach (IntraTurnObj itObj in ITObjects)
		{
			itObj.Deactivate ();
		}
		return true;
	}

	public bool CheckLevelComplete()
	{
		// return true if all enemies are disabled
		foreach (Entity e in entitiesList)
		{
			if (e.gameObject.activeSelf)
				return false;
		}
		Game.Save ();
		return true;
	}

	public void CreateEntities(List<EnemyStats> entities, GameObject entityPrefab)
	{
		foreach (EnemyStats e in entities)
		{
			GameObject o = Instantiate(entityPrefab);
			o.transform.SetParent(this.transform);
			Enemy enemy = o.GetComponent<Enemy>();
			enemy.InitStats(e);

			entitiesList.Add(enemy);
			level.PlaceEntity(enemy);
		}
	}

	public void CreatePlayers(List<HeroStats> players, GameObject entityPrefab)
	{
		foreach (HeroStats e in players)
		{
			GameObject o = Instantiate(entityPrefab);
			o.transform.SetParent(this.transform);
			HeroEntity player = o.GetComponent<HeroEntity>();
			player.InitStats(e);

			playerList.Add(player);
			level.PlaceEntity(player);
		}
	}
}
