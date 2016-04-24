using UnityEngine;
using System.Collections;

public class PlayerEffector : IntraTurnObj {

	// TODO fix this whole class
	public SimpleAnimPlayer anim;
	private HeroEntity target;
	private int damage;

	public void Init(HeroEntity target, int damage, Coords pos)
	{
		this.pos = pos;
		this.target = target;
		this.damage = damage;
		anim.Init ();
	}

	public override void Activate()
	{
		transform.position = Level.WorldCoord2Position(pos);
		done = false;
		gameObject.SetActive(true);
		anim.onFinish += Done;
		anim.Play(anim.anim);
	}

	public override void Deactivate()
	{
		gameObject.SetActive(false);
		anim.Reset();
	}

	public void Done()
	{
		target.stats.health -= damage;
		done = true;
	}
}
