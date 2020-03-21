using System.Collections;
using UnityEngine;

public class WorldGenerator {
	// /// <summary>
	// /// Generates the terrain using cell automata.
	// /// </summary>
	// /// <returns>The array for the terrain</returns>
	// /// <param name="size">Size.</param>
	// public int[,] GenerateTerrain(int size)
	// {
	// 	int[,] map = new int[size, size];
	// 	randomizeArray(map, 0, 1, 0.45f);   // Generate random noise
	// 	reduceNoise(ref map, 1, 10);      // Reduce noise
	// 	checkNeighbors(ref map, World.GRASS, World.SAND, World.WATER, 2);

	// 	overlay(ref map, World.FOREST, World.GRASS, 0.4f, 2);
	// 	overlay(ref map, World.MOUNTAIN, World.GRASS, 0.38f, 2);
	// 	checkNeighbors(ref map, World.MOUNTAIN, World.DIRT, World.GRASS, 3);

	// 	return map;
	// }

	/// <summary>
	/// Do a pass on each grid cell. Count the neighbors that are of type `check`
	/// If the number of neighbors exceeds `count`, then set this grid cell to `set`
	/// </summary>
	/// <param name="oldID">ID of the tile to replace in the old array</param> 
	/// <param name="newID">ID of the tile to replace the old one, if conditions are met</param>
	/// <param name="checkID">ID of the tile to check when looking at neighbors</param>
	/// <param name="count">Threshold for the number of neighbors</param>
	public void checkNeighbors(ref int[, ] arr, int oldID, int newID, int checkID, int count) {
		int[, ] answer = new int[arr.GetLength(0), arr.GetLength(1)];
		for (int y = 0; y < arr.GetLength(0); y++) {
			for (int x = 0; x < arr.GetLength(1); x++) {
				if (arr[y, x] == oldID && countNeighbors(arr, x, y, checkID) >= count)
					answer[y, x] = newID;
				else
					answer[y, x] = arr[y, x];
			}
		}
		for (int y = 0; y < arr.GetLength(0); y++)
			for (int x = 0; x < arr.GetLength(1); x++)
				arr[y, x] = answer[y, x];
	}

	/// <summary>
	/// Overlay the specified id, prob, noiseReductionReps and overlayOnTo.
	/// </summary>
	/// <param name="id">ID of tile to generate.</param>
	/// <param name="prob">Higher prob means denser blobs generated.</param>
	/// <param name="noiseReductionReps">Noise reduction reps. Less = more randomized/jagged</param>
	/// <param name="overlayOnTo">ID of tile to overlay on to.</param>
	public void overlay(ref int[, ] arr, ref int[, ] destArr, int id, int overlayOnTo, float prob, int noiseReductionReps) {
		int[, ] overlay = new int[arr.GetLength(0), arr.GetLength(1)];
		randomizeArray(overlay, id, 0, prob);
		reduceNoise(ref overlay, id, noiseReductionReps);

		for (int y = 0; y < arr.GetLength(0); y++) {
			for (int x = 0; x < arr.GetLength(1); x++) {
				if (arr[y, x] == overlayOnTo && overlay[y, x] == id)
					destArr[y, x] = id;
			}
		}
	}

	// places random values into an array
	public void randomizeArray(int[, ] arr, int one, int two, float prob) {
		for (int r = 0; r < arr.GetLength(0); r++) {
			for (int c = 0; c < arr.GetLength(1); c++) {
				if (Random.value < prob)
					arr[r, c] = one;
				else
					arr[r, c] = two;
			}
		}
	}

	public void reduceNoise(ref int[, ] arr, int check, int iterations = 1) {
		for (int i = 0; i < iterations; i++)
			reduceNoise(ref arr, check);
	}

	// use cell automata to reduce noise
	private void reduceNoise(ref int[, ] arr, int check) {
		int[, ] answer = new int[arr.GetLength(0), arr.GetLength(1)];
		for (int y = 0; y < arr.GetLength(0); y++) {
			for (int x = 0; x < arr.GetLength(1); x++) {
				int wallNeighbors = countNeighbors(arr, x, y, check);
				if (wallNeighbors >= 5)
					answer[y, x] = check;
			}
		}
		for (int y = 0; y < arr.GetLength(0); y++)
			for (int x = 0; x < arr.GetLength(1); x++)
				arr[y, x] = answer[y, x];
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
	private int countNeighbors(int[, ] arr, int x, int y, int check) {
		int counter = 0;
		for (int yy = -1; yy <= 1; yy++) {
			for (int xx = -1; xx <= 1; xx++) {
				// coords in array to be checked
				int xCheck = x + xx;
				int yCheck = y + yy;
				if (0 <= xCheck && xCheck < arr.GetLength(1) &&
					0 <= yCheck && yCheck < arr.GetLength(0) &&
					arr[y + yy, x + xx] == check)
					counter++;
			}
		}
		return counter;
	}
}