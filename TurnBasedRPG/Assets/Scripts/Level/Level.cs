using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Level : MonoBehaviour {

	public int gridSize = 10;

	// grid where terrain logic is performed
	public int[,] grid;
	// visualization of grid
	private SpriteRenderer[,] spriteMap;
	public Sprite[] sprites;

	// entity positions
	public Entity[,] entities;

	private LevelGenerator lGenerator = new LevelGenerator();

	// TODO implement object pooling for grid prefabs
	public GameObject gridPrefab;

	public void Init()
	{
		entities = new Entity[gridSize, gridSize];
		spriteMap = new SpriteRenderer[gridSize, gridSize];
		CreateMapGameObjects();
	}

	public void Reset()
	{
		for (int x = 0; x < gridSize; x++)
		{
			for (int y = 0; y < gridSize; y++)
			{
				entities [y, x] = null;
			}
		}
	}

	public void GenerateLevel()
	{
		grid = lGenerator.GenerateLevel(gridSize);
		UpdateSpriteMap();
	}

	/// <summary>
	/// Update the sprites in the game according to the grid
	/// </summary>
	private void UpdateSpriteMap()
	{
		for (int x = 0; x < gridSize; x ++)
		{
			for (int y = 0; y < gridSize; y ++)
			{
				int spriteIndex = grid[y, x];
				spriteMap[y, x].sprite = sprites[spriteIndex];
			}
		}
	}

	/// <summary>
	/// instantiate all map prefab objects
	/// </summary>
	private void CreateMapGameObjects()
	{ 
		for (int x = 0; x < gridSize; x ++)
		{
			for (int y = 0; y < gridSize; y ++)
			{
				GameObject o = Instantiate(gridPrefab);
				o.transform.SetParent(this.transform);
				o.transform.localPosition = new Vector2(x, y);
				spriteMap[y, x] = o.GetComponent<SpriteRenderer>();
			}
		}
	}

	public void PlaceEntity(Entity e)
	{
		// place the entity on a walkable position
		e.pos = Coords.RandomPos(gridSize);
		while (getTerrain(e.pos) != 0 || getEntity(e.pos) != null)
			e.pos = Coords.RandomPos(gridSize);

		entities[e.pos.y, e.pos.x] = e;
		e.transform.localPosition = new Vector2(e.pos.x, e.pos.y);
		e.Init(this);
	}

	public List<int> getAdjacentTerrain(Coords pos)
	{
		List<int> answer = new List<int>();
		foreach (Coords c in getAdjacentCoords(pos))
			answer.Add(getTerrain(c));
		return answer;
	}

	public List<Entity> getAdjacentEntities(Coords pos)
	{
		List<Entity> answer = new List<Entity>();
		foreach (Coords c in getAdjacentCoords(pos))
			answer.Add(getEntity(c));
		return answer;
	}

	public List<Coords> getAdjacentCoords(Coords pos)
	{
		List<Coords> answer = new List<Coords>();
		foreach (Coords c in pos.AdjacentCoords())
		{
			if (inBounds(c))
				answer.Add(c);
		}
		return answer;
	}

	public int getTerrain(Coords pos)
	{
		return grid[pos.y, pos.x];
	}

	public Entity getEntity(Coords pos)
	{
		return entities[pos.y, pos.x];
	}

	public bool inBounds(Coords pos)
	{
		return pos.x >= 0 && pos.x < gridSize &&
			pos.y >= 0 && pos.y < gridSize;
	}

	public void Highlight(Coords pos)
	{
		spriteMap[pos.y, pos.x].color = new Color(.5f, .5f, .5f);
	}

	public void UnHighlight(Coords pos)
	{
		spriteMap[pos.y, pos.x].color = new Color(1, 1, 1);
	}

	public static Vector2 WorldCoord2Position(Coords pos)
	{
		return new Vector2(pos.x, pos.y);
	}
}
