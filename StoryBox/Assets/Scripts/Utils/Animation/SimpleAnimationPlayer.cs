using UnityEngine;
using System.Collections;

public class SimpleAnimationPlayer : MonoBehaviour
{
	public SpriteRenderer sr;
	public SimpleAnimation anim;
	public bool isPlaying { get; private set; }

	private int frameIndex;
	private Coroutine playAnimRoutine;

	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	public void Play()
	{
		if (anim == null)
			return;
		if (playAnimRoutine != null)
			StopCoroutine(playAnimRoutine);
		playAnimRoutine = StartCoroutine(PlayAnim());
	}

	protected virtual IEnumerator PlayAnim()
	{
		frameIndex = 0;
		isPlaying = true;
		while (frameIndex < anim.frames.Length)
		{
			sr.sprite = anim.frames[frameIndex];
			frameIndex++;
			// if looping, set animation to beginning and do not stop playing
			if (anim.looping && frameIndex >= anim.frames.Length) {
				frameIndex = 0;
			}
			yield return new WaitForSecondsRealtime(anim.SecondsPerFrame);
		}
		isPlaying = false;
		yield return null;
	}

	protected virtual void OnAnimationEnded() {}
}