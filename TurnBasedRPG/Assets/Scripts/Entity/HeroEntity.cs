using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroEntity : Entity {

	public HeroStats stats;
	public HeroView view;
	public EffectorView effectorView;

	public ActionGrid actionGrid = new ActionGrid();
	public List<Coords> movement = new List<Coords>();

	//public int movementRange = 3;

	/// <summary>
	/// 0 = move, 1 = attack1, 2 = attack2, ... etc.
	/// </summary>
	public int actionMode;

	public override void Init(Level level)
	{
		base.Init (level);

		view.Init (stats);
		effectorView.Init (stats.weapon);

		foreach(EnemyEffector effector in effectorView.effectors)
		{
			LevelManager.instance.ITObjects.Add (effector);
		}
	}

	public override void Act()
	{
		StartCoroutine ("GetInput");
	}

	public void ChangeActionMode(int mode)
	{
		actionMode = mode;
		if (actionMode == 0)
		{
			actionGrid.SetParams (ActionGrid.Shape.Circle,
				stats.moveRange,
				false,
				true,
				false);
		}
		else
		{
			EnemyEffector effector = effectorView.effectors [actionMode - 1];
			if (effector.stats.type == EffectorStats.EffectorType.Projectile)
				actionGrid.SetParams (ActionGrid.Shape.Cross,
					effector.stats.range,
					true,
					true,
					false);
			else if (effector.stats.type == EffectorStats.EffectorType.Point)
				actionGrid.SetParams (ActionGrid.Shape.Circle,
					effector.stats.range,
					true,
					false,
					false);
		}
		UnHighlightActionCoords ();
		actionGrid.GetActionCoords(level, pos);
		HighlightActionCoords();
	}

	private IEnumerator GetInput()
	{
		ChangeActionMode (0);
		bool gotInput = false;
		while (!gotInput)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				int mouseX = Mathf.RoundToInt(mousePos.x);
				int mouseY = Mathf.RoundToInt(mousePos.y);

				if (level.inBounds(new Coords(mouseX, mouseY)) &&
					actionGrid.grid[mouseY, mouseX] != 0)
				{
					if (actionMode == 0)
						Move(new Coords(mouseX, mouseY));
					else
						Shoot(new Coords(mouseX, mouseY));
					
					gotInput = true;
					UnHighlightActionCoords();
				}
			}
			yield return null;
		}

		/*bool ITObjectsDone = false;
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
		//Debug.Log("ITObject done");*/

		done = true;
		yield return null;
	}

	// sets the movement list to a list of possible coordinates to move to
	/*private void GetMovementCoords()
	{
		// Breadth-first search
		List<Coords> frontier = new List<Coords>();

		for (int x = 0; x < level.gridSize; x++)
			for (int y = 0; y < level.gridSize; y++)
				actionGrid[y, x] = -1;

		frontier.Add(pos);
		actionGrid[pos.y, pos.x] = 0;

		// keep going until no more frontier
		while (frontier.Count > 0)
		{
			// get the current frontier
			Coords current = frontier[0];
			frontier.RemoveAt(0);

			// if we have already searched to the max range, do not continue
			if (actionGrid[current.y, current.x] < movementRange)
			{
				// look at each neighbor of the frontier
				foreach (Coords neighbor in level.getAdjacentCoords(current))
				{
					// if the terrain at the neighbor is clear
					if (actionGrid[neighbor.y, neighbor.x] == -1 &&
						level.getTerrain(neighbor) == 0 &&
						level.getEntity(neighbor) == null)
					{
						actionGrid[neighbor.y, neighbor.x] = actionGrid[current.y, current.x] + 1;
						frontier.Add(neighbor);
					}
				}
			}
		}
	}*/

	private void Shoot(Coords target)
	{
		EnemyEffector effector = effectorView.effectors [actionMode - 1];
		//Debug.Log(target);
		if (effector.stats.type == EffectorStats.EffectorType.Projectile)
		{
			effector.dir = new Coords (target.x - pos.x, target.y - pos.y).Normalized ();
			effector.Init (new Coords (pos.x + effector.dir.x, pos.y + effector.dir.y), level);
		}
		else if (effector.stats.type == EffectorStats.EffectorType.Point)
		{
			effector.Init (target, level);
		}
		effector.Activate();
	}

	private void HighlightActionCoords()
	{
		for (int x = 0; x < level.gridSize; x ++)
		{
			for (int y = 0; y < level.gridSize; y ++)
			{
				if (actionGrid.grid[y, x] != 0)
					level.Highlight(new Coords(x, y));
			}
		}
	}

	private void UnHighlightActionCoords()
	{
		for (int x = 0; x < level.gridSize; x++)
		{
			for (int y = 0; y < level.gridSize; y++)
			{
				level.UnHighlight(new Coords(x, y));
			}
		}
	}

	public void InitStats(HeroStats stats)
	{
		this.stats = stats;
	}
}
