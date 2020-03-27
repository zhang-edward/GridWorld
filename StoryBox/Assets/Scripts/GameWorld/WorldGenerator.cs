using System.Collections;
using UnityEngine;

public class WorldGenerator {

	public void GeneratePerlinNoiseMap(ref int[,] arr, int worldSize, float[] thresholds) {
		int xOffset = Random.Range(0, worldSize);
		int yOffset = Random.Range(0, worldSize);
		for (int y = 0; y < worldSize; y++) {
			for (int x = 0; x < worldSize; x++) {
				float yy = (float)y / worldSize * 4f + yOffset;
				float xx = (float)x / worldSize * 4f + xOffset;
				float noise = Mathf.Max(Mathf.PerlinNoise(xx, yy) - CircleGradient(x, y, worldSize) * 1.25f, 0);
				arr[y, x] = CalculateThreshold(noise, thresholds);
			}
		}
	}

	private int CalculateThreshold(float val, float[] thresholds) {
		int i;
		for (i = 0; i < thresholds.Length; i++) {
			if (val < thresholds[i])
				return i;
		}
		return i;
	}

	private float CircleGradient(float x, float y, float size) {
		return (new Vector2(x - (size / 2), y - (size / 2))).magnitude / size;
	}

	/// <summary>
	/// Do a pass on each grid cell. Count the neighbors that are of type `check`
	/// If the number of neighbors exceeds `count`, then set this grid cell to `set`
	/// </summary>
	/// <param name="oldID">ID of the tile to replace in the old array</param> 
	/// <param name="newID">ID of the tile to replace the old one, if conditions are met</param>
	/// <param name="checkID">ID of the tile to check when looking at neighbors</param>
	/// <param name="count">Threshold for the number of neighbors</param>
	public void CheckNeighbors(ref int[, ] arr, int oldID, int newID, int checkID, int count) {
		int[, ] answer = new int[arr.GetLength(0), arr.GetLength(1)];
		for (int y = 0; y < arr.GetLength(0); y++) {
			for (int x = 0; x < arr.GetLength(1); x++) {
				if (arr[y, x] == oldID && CountNeighbors(arr, x, y, checkID) >= count)
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
	public void Overlay(ref int[, ] arr, ref int[, ] destArr, int id, int overlayOnTo, float prob, int noiseReductionReps) {
		int[, ] overlay = new int[arr.GetLength(0), arr.GetLength(1)];
		RandomizeArray(overlay, id, 0, prob);
		ReduceNoise(ref overlay, id, noiseReductionReps);

		for (int y = 0; y < arr.GetLength(0); y++) {
			for (int x = 0; x < arr.GetLength(1); x++) {
				if (arr[y, x] == overlayOnTo && overlay[y, x] == id)
					destArr[y, x] = id;
			}
		}
	}

	// places random values into an array
	public void RandomizeArray(int[, ] arr, int one, int two, float prob) {
		for (int r = 0; r < arr.GetLength(0); r++) {
			for (int c = 0; c < arr.GetLength(1); c++) {
				if (Random.value < prob)
					arr[r, c] = one;
				else
					arr[r, c] = two;
			}
		}
	}

	public void ReduceNoise(ref int[, ] arr, int check, int iterations = 1) {
		for (int i = 0; i < iterations; i++)
			ReduceNoise(ref arr, check);
	}

	// use cell automata to reduce noise
	private void ReduceNoise(ref int[, ] arr, int check) {
		int[, ] answer = new int[arr.GetLength(0), arr.GetLength(1)];
		for (int y = 0; y < arr.GetLength(0); y++) {
			for (int x = 0; x < arr.GetLength(1); x++) {
				int wallNeighbors = CountNeighbors(arr, x, y, check);
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
	private int CountNeighbors(int[, ] arr, int x, int y, int check) {
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