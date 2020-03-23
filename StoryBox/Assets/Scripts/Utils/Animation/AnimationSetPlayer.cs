using UnityEngine;
using System.Collections;

public class AnimationSetPlayer : SimpleAnimationPlayer
{
	public AnimationSet animationSet;
	public string currentAnimation { get; private set; }

	public bool Play(string name)
	{
		anim = animationSet.dict[name];
		currentAnimation = name;
		Play();
		return true;
	}

	public void ResetToDefault()
	{
		currentAnimation = "Default";
		anim = animationSet["Default"];
		Play();
	}

	protected override void OnAnimationEnded() {
		ResetToDefault();
	}
}
