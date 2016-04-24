using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EntityInfoPanel : MonoBehaviour {

	public Image image;
	public Text health;
	public Slider healthSlider;
	public Text Level;
	public Slider experienceSlider;
	public Text strength;
	public Text currentQuest;

	public LivingEntity entity;

	public void Init(LivingEntity entity)
	{
		this.entity = entity;
	}

	public void SetValues()
	{
		health.text = "Health: " + entity.defs.health;

		healthSlider.maxValue = entity.defs.maxHealth;
		healthSlider.minValue = 0;
		healthSlider.value = entity.defs.health;

		Level.text = "" + entity.defs.Level;
		experienceSlider.maxValue = 100;
		experienceSlider.minValue = 0;
		experienceSlider.value = entity.defs.experience % 100;

		strength.text = "Strength: " + entity.combatDefs.strength;
		if (entity is Hero)
		{
			Hero hero = entity as Hero;
			if (hero.currentQuest != null)
				currentQuest.text = hero.currentQuest.ToString();
			else
				currentQuest.text = "None";
		}
	}

	void Update()
	{
		SetValues();
	}
}
