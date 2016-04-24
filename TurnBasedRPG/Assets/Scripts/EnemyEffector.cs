using UnityEngine;
using System.Collections;

public class EnemyEffector : IntraTurnObj
{
	public Coords dir;

	public EffectorStats stats;
	private int life;

	// the sprite of the projectile
	private SpriteRenderer sr;
	//private Sprite sprite;

	// the animator that this effector will play on impact or death
	public SimpleAnim SpawnedAnim;
	public SimpleAnim InAirAnim;
	public SimpleAnim OnImpactAnim;
	public SimpleAnimPlayer animPlayer;

	// Level grid to process position
	private Level level;

	void Awake()
	{
		sr = GetComponent<SpriteRenderer> ();
		sr.sprite = SpawnedAnim.frames[0];
	}

	public void Init(Coords pos, Level level)
	{
		this.pos = pos;
		this.level = level;
	}

	public override void Activate()
	{
		// set position and angle
		transform.position = Level.WorldCoord2Position(pos);
		float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

		// set the projectile type
		if (stats.type == EffectorStats.EffectorType.Projectile)
			life = stats.range - 1;
		else if (stats.type == EffectorStats.EffectorType.Point)
			life = 1;

		// do other stuff
		done = false;
		gameObject.SetActive(true);
		//sr.enabled = true;
		StartCoroutine("Act");
	}

	public override void Deactivate()
	{
		gameObject.SetActive(false);
		animPlayer.Reset();
	}

	public void Done()
	{
		animPlayer.onFinish -= Done;
		done = true;
	}

	public IEnumerator Act()
	{
		// Play Spawned animation
		animPlayer.Play (SpawnedAnim);
		while (animPlayer.isPlaying)
			yield return null;
		animPlayer.Reset ();

		// Play In Air animation
		animPlayer.Play (InAirAnim);
		animPlayer.Reset ();
		// check if the lifespan ran out or if the projectile hit something
		while (!CheckImpact())
		{
			// update the position by adding the dir
			pos = new Coords(pos.x + dir.x, pos.y + dir.y);

			// smooth movement of the bullet using lerp
			float t = 0;
			Vector2 start = transform.position;
			Vector2 dest = Level.WorldCoord2Position(pos);
			while (!updatePosition(start, dest, t))
			{
				t += Time.deltaTime * 20f;
				yield return null;
			}
			// decrease lifespan
			life--;
		}

		// Play the Impact Animation
		animPlayer.onFinish += Done;
		animPlayer.Play (OnImpactAnim);

		yield return null;
	}

	public bool updatePosition(Vector2 start, Vector2 destination, float t)
	{
		transform.position = Vector2.Lerp(start, destination, t);
		if (Vector2.Distance(transform.position, destination) < 0.01)
		{
			transform.position = destination;
			return true;
		}
		else
			return false;
	}

	public bool CheckImpact()
	{
		if (life <= 0)
		{
			/*Debug.Log ("hello");*/
			return true;
		}
		
		Entity e = level.getEntity(pos);
		if (e != null || level.getTerrain(pos) == 1)
		{
			if (e is Enemy)
			{
				Enemy enemy = (Enemy)e;
				// TODO: call method in enemy which damages enemy
				enemy.stats.health -= stats.damage;
			}
//			Debug.Log (e);
//			Debug.Log (level.getTerrain (pos));
			return true;
		}
		return false;
	}
}
