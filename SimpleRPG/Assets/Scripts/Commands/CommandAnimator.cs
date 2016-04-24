using UnityEngine;
using System.Collections;

public class CommandAnimator : MonoBehaviour {

	private Animator anim;
	private SpriteRenderer sr;

	void Awake()
	{
		anim = GetComponentInChildren<Animator>();
		sr = GetComponentInChildren<SpriteRenderer>();
	}

	void Update()
	{
	}

	public void Animate(string name)
	{
		transform.rotation = Quaternion.identity;
		anim.SetTrigger(name);
	}

	public void Animate(string name, Vector2 target)
	{
		Vector3 dir = (Vector3)target - transform.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		anim.SetTrigger(name);
	}
}
