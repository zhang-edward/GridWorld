using UnityEngine;
using System.Collections;

public class SimpleAnimationPlayer : MonoBehaviour
{
	public SpriteRenderer spriteRenderer;
	public SimpleAnimation anim;
	public bool isPlaying { get; private set; }

	private int frameIndex;
	private Coroutine playAnimRoutine;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void OnEnable()
	{
		//sr.sprite = anim.frames [0];
		Play();
	}

	public void Play()
	{
		if (anim == null)
			return;
		if (playAnimRoutine != null)
			StopCoroutine(playAnimRoutine);
		playAnimRoutine = StartCoroutine(PlayAnim());
	}

	public void Reset()
	{
		frameIndex = 0;
		spriteRenderer.sprite = anim.frames[0];
		if (playAnimRoutine != null)
			StopCoroutine(playAnimRoutine);
	}

	protected virtual IEnumerator PlayAnim()
	{
		isPlaying = true;
		while (frameIndex < anim.frames.Length)
		{
			spriteRenderer.sprite = anim.frames[frameIndex];
			frameIndex++;
			// if looping, set animation to beginning and do not stop playing
			if (anim.looping)
			{
				if (frameIndex >= anim.frames.Length)
					frameIndex = 0;
			}
			yield return new WaitForSecondsRealtime(anim.SecondsPerFrame);
		}
		isPlaying = false;
		yield return null;
	}
}