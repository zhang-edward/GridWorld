using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Coords {
	public int x;
	public int y;

	public Coords(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public Coords[] AdjacentCoords()
	{
		Coords[] answer = new Coords[4];
		answer[0] = new Coords(x, y + 1);
		answer[1] = new Coords(x + 1, y);
		answer[2] = new Coords(x, y - 1);
		answer[3] = new Coords(x - 1, y);
		return answer;
	}

	public Coords Normalized()
	{
		int xx = (int)Mathf.Sign(x);
		int yy = (int)Mathf.Sign(y);
		if (x == 0)
			xx = 0;
		if (y == 0)
			yy = 0;
		return new Coords(xx, yy);
	}

	/// <summary>
	/// Manhattan distance between two coords
	/// </summary>
	/// <param name="one"></param>
	/// <param name="two"></param>
	/// <returns>Distance</returns>
	public static int Distance(Coords one, Coords two)
	{
		return Mathf.Abs(one.x - two.x) + Mathf.Abs(one.y - two.y);
	}

	public static Coords RandomPos(int size)
	{
		int x = UnityEngine.Random.Range(0, size);
		int y = UnityEngine.Random.Range(0, size);
		return new Coords(x, y);
	}

	public override string ToString()
	{
		return "(" + x + ", " + y + ")";
	}
}
