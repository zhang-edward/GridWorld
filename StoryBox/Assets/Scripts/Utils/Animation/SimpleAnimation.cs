using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class SimpleAnimation
{
	public Sprite[] frames;
	[Space]
	public float fps = 10;
	public bool looping;

	public float SecondsPerFrame
	{
		get { return 1.0f / (float)fps; }
	}
	
	public float TimeLength
	{
		get { return frames.Length * (float)SecondsPerFrame; }
	}

	public void SetTimeLength(float timeLength)
	{
		fps = 1 / (timeLength / frames.Length);
	}

	public float GetSecondsUntilFrame(int frame)
	{
		return frame * SecondsPerFrame;
	}
}