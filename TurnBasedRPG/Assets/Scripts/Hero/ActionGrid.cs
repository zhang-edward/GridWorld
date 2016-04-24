using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionGrid {

	public delegate void GetCoords(Level level, Coords pos);
	public GetCoords GetActionCoords;

	public enum Shape
	{
		Circle,
		Cross
	}
	private Shape shape;
	private bool includeEntities;
	private bool includeEmptyTerrain;
	private int range;
	private bool passWalls;

	public int[,] grid;

	public void SetParams(Shape shape, int range, bool includeEntities, bool includeEmptyTerrain, bool passWalls)
	{
		this.shape = shape;
		this.range = range;
		this.includeEntities = includeEntities;
		this.includeEmptyTerrain = includeEmptyTerrain;
		this.passWalls = passWalls;

		if (this.shape == Shape.Circle)
			GetActionCoords += GetActionCoordsCircle;
		else if (this.shape == Shape.Cross)
			GetActionCoords += GetActionCoordsCross;
	}

	private void GetActionCoordsCross(Level level, Coords pos)
	{
		grid = new int[level.gridSize, level.gridSize];
		
		for (int i = pos.x; i < level.gridSize; i ++)
		{
			Coords neighbor = new Coords(i, pos.y);
			bool[] values = ProcessCoord(neighbor, level);
			bool searchable = values[0];    // whether this neighbor should be added to the frontier
			bool actionable = values[1];    // whether this neighbor should be added to the actionGrid

			if (actionable)
				grid[neighbor.y, neighbor.x] = 1;
			if (!searchable || (i - pos.x) >= range)
			{
				break;
			}
		}

		for (int i = pos.x - 1; i >= 0; i--)
		{
			Coords neighbor = new Coords(i, pos.y);
			bool[] values = ProcessCoord(neighbor, level);
			bool searchable = values[0];    // whether this neighbor should be added to the frontier
			bool actionable = values[1];    // whether this neighbor should be added to the actionGrid

			if (actionable)
				grid[neighbor.y, neighbor.x] = 1;
			if (!searchable || (pos.x - i) >= range)
			{
				break;
			}
		}

		for (int i = pos.y + 1; i < level.gridSize; i++)
		{
			Coords neighbor = new Coords(pos.x, i);
			bool[] values = ProcessCoord(neighbor, level);
			bool searchable = values[0];    // whether this neighbor should be added to the frontier
			bool actionable = values[1];    // whether this neighbor should be added to the actionGrid

			if (actionable)
				grid[neighbor.y, neighbor.x] = 1;
			if (!searchable || (i - pos.y) >= range)
				break;
		}

		for (int i = pos.y - 1; i >= 0; i--)
		{
			Coords neighbor = new Coords(pos.x, i);
			bool[] values = ProcessCoord(neighbor, level);
			bool searchable = values[0];    // whether this neighbor should be added to the frontier
			bool actionable = values[1];    // whether this neighbor should be added to the actionGrid

			if (actionable)
				grid[neighbor.y, neighbor.x] = 1;
			if (!searchable || (pos.y - i) >= range)
				break;
		}
	}

	// gets coords in a circle in a given range
	private void GetActionCoordsCircle(Level level, Coords pos)
	{
		int[,] searchGrid = new int[level.gridSize, level.gridSize];
		grid = new int[level.gridSize, level.gridSize];

		for (int x = 0; x < level.gridSize; x++)
			for (int y = 0; y < level.gridSize; y++)
				searchGrid[y, x] = -1;

		// Breadth-first search
		List<Coords> frontier = new List<Coords>();
		frontier.Add(pos);

		searchGrid[pos.y, pos.x] = 0;

		// keep going until no more frontier
		while (frontier.Count > 0)
		{
			// get the current frontier
			Coords current = frontier[0];
			frontier.RemoveAt(0);

			// if we have already searched to the max range, do not continue
			if (searchGrid[current.y, current.x] < range)
			{
				// look at each neighbor of the frontier
				foreach (Coords neighbor in level.getAdjacentCoords(current))
				{
					// check if we have processed this square already
					if (searchGrid[neighbor.y, neighbor.x] == -1)
					{
						bool[] values = ProcessCoord(neighbor, level);
						bool searchable = values[0];    // whether this neighbor should be added to the frontier
						bool actionable = values[1];    // whether this neighbor should be added to the actionGrid

						if (searchable)
						{
							searchGrid[neighbor.y, neighbor.x] = searchGrid[current.y, current.x] + 1;
							frontier.Add(neighbor);
						}
						if (actionable)
						{
							grid[neighbor.y, neighbor.x] = 1;
						}
					}
				}
			}
		}
	}

	private bool[] ProcessCoord(Coords pos, Level level)
	{
		bool[] answer = new bool[2];
		bool searchable = true;
		bool actionable = true;

		// if we are excluding empty terrain and there is terrain without an entity (empty terrain)
		if (!includeEmptyTerrain && level.getEntity(pos) == null)
		{
			actionable = false;
		}
		// if we are excluding entities and there is terrain with an entity
		if (!includeEntities && level.getEntity(pos) != null)
		{
			actionable = false;
		}
		// if we are passing walls, do not add walls to actionGrid but add to the frontier
		if (passWalls)
		{
			if (level.getTerrain(pos) == 1)
			{
				actionable = false;
			}
		}
		// if we are not passing walls, do not process walls
		else
		{
			if (level.getTerrain(pos) == 1)
			{
				searchable = false;
				actionable = false;
			}
		}
		answer[0] = searchable;
		answer[1] = actionable;
		return answer;
	}
}
