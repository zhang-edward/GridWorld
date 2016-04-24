using UnityEngine;
using System.Collections;

public class SpriteIDManager : MonoBehaviour {

	public static SpriteIDManager instance;

	[Header("Effects")]
	public SimpleAnim[] SpawnAnimations;
	public SimpleAnim[] InAirAnimations;
	public SimpleAnim[] ImpactAnimations;
	//public Sprite[] bulletSprites;

	[Header("Weapon")]
	public Sprite[] staffSprites;
	public Sprite[] wandSprites;
	public Sprite[] orbSprites;

	[Header("Body")]
	public Sprite[] bodySprites;

	public void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
	}

	public SimpleAnim[] getRandomSimpleAnim()
	{
		SimpleAnim[] answer = new SimpleAnim[3];
		int randIndex = Random.Range (0, SpawnAnimations.Length);

		answer[0] = SpawnAnimations [randIndex];
		answer [1] = InAirAnimations [randIndex];
		answer [2] = ImpactAnimations [randIndex];
		return answer;
	}
}
