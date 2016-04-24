using UnityEngine;
using System.Collections;

public class WorldGenerator : MonoBehaviour {

	/* KEY:
	 * Water = 0
	 * Grass = 1
	 * Dirt = 2
	 * Sand = 3 (Beach)
	 * Forest = 4
	 * Mountain = 5
	 * */


	/// <summary>
	/// Generates the terrain using cell automata.
	/// </summary>
	/// <returns>The array for the terrain</returns>
	/// <param name="size">Size.</param>
	public int[,] GenerateTerrain(int size)
	{
		int[,] map = new int[size, size];
		randomizeArray(map, 0, 1, 0.45f);

		for (int i = 0; i <= 10; i ++)
			map = reduceNoise(map, 1);

		for (int y = 0; y < size; y ++)
		{
			for (int x = 0; x < size; x ++)
			{
				if (map[y, x] == 1 &&
				    countNeighbors(map, x, y, 0) >= 3)
					map[y, x] = 3;
			}
		}

		overlay (map, 4, 1, 0.4f, 2);
		overlay (map, 5, 1, 0.38f, 2);

		for (int y = 0; y < size; y ++)
		{
			for (int x = 0; x < size; x ++)
			{
				if (map[y, x] == 5 &&
				    countNeighbors(map, x, y, 1) >= 3)
					map[y, x] = 2;
			}
		}

		/*int[,] forest = new int[size, size];
		randomizeArray(forest, 4, 0, 0.42f);

		for (int i = 0; i <= 2; i ++)
			forest = reduceNoise(forest, 4);

		for (int y = 0; y < size; y ++)
		{
			for (int x = 0; x < size; x ++)
			{
				if (map[y, x] == 1 && forest[y, x] == 4)
					map[y, x] = 4;
			}
		}*/



		return map;
	}

	/// <summary>
	/// Overlay the specified id, prob, noiseReductionReps and overlayOnTo.
	/// </summary>
	/// <param name="id">ID of tile to generate.</param>
	/// <param name="prob">Higher prob means denser blobs generated.</param>
	/// <param name="noiseReductionReps">Noise reduction reps. Less = more randomized/jagged</param>
	/// <param name="overlayOnTo">ID of tile to overlay on to.</param>
	private void overlay(int[,] arr, int id, int overlayOnTo, float prob, int noiseReductionReps)
	{
		int[,] overlay = new int[arr.GetLength(0), arr.GetLength(1)];
		randomizeArray(overlay, id, 0, prob);
		
		for (int i = 0; i <= 2; i ++)
			overlay = reduceNoise(overlay, id);
		
		for (int y = 0; y < arr.GetLength(0); y ++)
		{
			for (int x = 0; x < arr.GetLength(1); x ++)
			{
				if (arr[y, x] == 1 && overlay[y, x] == id)
					arr[y, x] = id;
			}
		}
	}

	// places random values into an array
	private void randomizeArray(int[,] arr, int one, int two, float prob)
	{
		for (int r = 0; r < arr.GetLength (0); r ++)
		{
			for (int c = 0; c < arr.GetLength(1); c ++)
			{
				if (Random.value < prob)
					arr[r, c] = one;
				else
					arr[r, c] = two;
			}
		}
	}

	// use cell automata to reduce noise
	private int[,] reduceNoise(int[,] arr, int check)
	{
		int[,] answer = new int[arr.GetLength(0), arr.GetLength(1)];
		for (int y = 0; y < arr.GetLength(0); y ++)
		{
			for (int x = 0; x < arr.GetLength(1); x ++)
			{
				int wallNeighbors = countNeighbors(arr, x, y, check);
				if (wallNeighbors >= 5)
					answer[y, x] = check;
			}
		}
		return answer;
	}

	/// <summary>
	/// Counts the neighbors.
	/// </summary>
	/// <returns>The number of neighbors.</returns>
	/// <param name="arr">The array.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="check">The int to count.</param>
	// note: this also checks the cell at x and y, not just neighbors
	private int countNeighbors(int[,] arr, int x, int y, int check)
	{
		int counter = 0;
		for (int yy = -1; yy <= 1; yy ++)
		{
			for (int xx = -1; xx <= 1; xx ++)
			{
				// coords in array to be checked
				int xCheck = x + xx;
				int yCheck = y + yy;
				if (0 <= xCheck && xCheck < arr.GetLength(1) &&
				    0 <= yCheck && yCheck < arr.GetLength(0) &&
				    arr[y + yy, x + xx] == check)
					counter ++;
			}
		}
		return counter;
	}
}
