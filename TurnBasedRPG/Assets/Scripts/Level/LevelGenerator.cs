using UnityEngine;
using System.Collections;

public class LevelGenerator {

	public int[,] GenerateLevel(int size)
	{
		int[,] answer = new int[size, size];
		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y ++)
			{
				if (x == 0 || x == size - 1 || //x == 3 ||
					y == 0 || y == size - 1)
					answer[y, x] = 1;
			}
		}
		return answer;
	}
}
