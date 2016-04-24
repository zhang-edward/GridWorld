using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class EffectorView : MonoBehaviour {

	public BaseWeapon weapon;

	// number of different actions this weapon can perform
	public List<EnemyEffector> effectors = new List<EnemyEffector>();
	public GameObject effectorPrefab;
	//public GameObject animPrefab;

	void Awake()
	{
		//effectorStats = new EffectorStats[numEffectors];
		//effectorStats [0] = (DebugRandomizeStats ());
		//effectorStats [1] = (DebugRandomizeStats ());
	}

	/*public EffectorStats DebugRandomizeStats()
	{
		EffectorStats answer = new EffectorStats ();
		answer.damage = Random.Range (1, 4);
		answer.range = Random.Range(3, 5);

		if (Random.value < 0.5f)
			answer.type = EffectorStats.EffectorType.Point;
		else
			answer.type = EffectorStats.EffectorType.Projectile;

		return answer;
	}*/

	public void Init(BaseWeapon weapon)
	{
		this.weapon = weapon;
		//GetComponent<SpriteRenderer> ().sprite = sprite;
		foreach (EffectorStats stats in weapon.effectorStats)
		{
			GameObject o = Instantiate (effectorPrefab) as GameObject;
			o.transform.SetParent (this.transform);

			EnemyEffector effector = o.GetComponent<EnemyEffector>();
			effector.stats = stats;
			//GameObject animObj = Instantiate (animPrefab, effector.transform.position, effector.transform.rotation) as GameObject;
			//animObj.transform.SetParent (effector.transform);
			//effector.animPlayer = animObj.GetComponent<SimpleAnimPlayer> ();

			// set Animations
			SimpleAnim[] animations = SpriteIDManager.instance.getRandomSimpleAnim ();
			effector.SpawnedAnim = animations [0];
			effector.InAirAnim = animations [1];
			effector.OnImpactAnim = animations [2];
			effector.animPlayer.anim = effector.SpawnedAnim;

			effector.animPlayer.Init ();

			effectors.Add (effector);
		}
	}
}
