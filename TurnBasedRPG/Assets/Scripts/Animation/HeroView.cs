using UnityEngine;
using System.Collections;

public class HeroView : MonoBehaviour {

	public Sprite bodySprite;
	public Sprite weaponSprite;

	public SpriteRenderer bodySR;
	public SpriteRenderer weaponSR;

	void Awake()
	{
//		Debug.Log (bodySR + ", " + weaponSR);
	}

	public void Init(HeroStats stats)
	{
		Sprite body = SpriteIDManager.instance.bodySprites [stats.bodyID];
		Sprite weapon = null;
		BaseWeapon.WeaponType weaponType = stats.weapon.weaponType;
		if (weaponType == BaseWeapon.WeaponType.Staff)
			weapon = SpriteIDManager.instance.staffSprites [stats.weapon.spriteID];
		else if (weaponType == BaseWeapon.WeaponType.Wand)
			weapon = SpriteIDManager.instance.wandSprites [stats.weapon.spriteID];
		else if (weaponType == BaseWeapon.WeaponType.Orb)
			weapon = SpriteIDManager.instance.orbSprites [stats.weapon.spriteID];

		this.bodySprite = body;
		this.weaponSprite = weapon;

		bodySR.sprite = this.bodySprite;
		weaponSR.sprite = this.weaponSprite;
	}
}
