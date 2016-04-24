using UnityEngine;
using System.Collections;

public class SimpleAnimPlayer : MonoBehaviour {

	public SpriteRenderer sr;
	public SimpleAnim anim;
	public delegate void OnFinishedAnimation();
	public OnFinishedAnimation onFinish;

	private float secondsPerFrame;
	private int frameIndex;

	public bool isPlaying;
	public bool looping;

	public void Init()
	{
		sr = GetComponent<SpriteRenderer>();
		secondsPerFrame = 1.0f / anim.fps;
		frameIndex = 0;
		sr.sprite = anim.frames [0];
	}

	public void Play(SimpleAnim anim)
	{
		this.anim = anim;
		//Debug.Log (anim.frames.Length);
		StartCoroutine("PlayAnim");
	}

	public void Reset()
	{
		//Debug.Log ("Reset:" + anim.frames.Length);
		frameIndex = 0;
		sr.sprite = anim.frames[0];
	}

	private IEnumerator PlayAnim()
	{
		isPlaying = true;
		while (frameIndex < anim.frames.Length)
		{
			sr.sprite = anim.frames[frameIndex];
			frameIndex++;
			// if looping, set animation to beginning and do not stop playing
			if (looping)
			{
				if (frameIndex >= anim.frames.Length)
					frameIndex = 0;
			}
			yield return new WaitForSeconds(secondsPerFrame);
		}
		isPlaying = false;
		if (onFinish != null)
			onFinish();
		yield return null;
	}
}
