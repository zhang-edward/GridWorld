using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerActionMenu : MonoBehaviour {

	public HeroEntity player;

	public PlayerActionButton[] buttons;

	void Start()
	{
		// initialize all the buttons with the correct action number
		for (int i = 0; i < buttons.Length; i ++)
		{
			buttons [i].Init (this, i + 1);
		}
	}

	public void Init(HeroEntity player)
	{
		this.player = player;
		// disable all buttons
		foreach (PlayerActionButton b in buttons)
		{
			b.Deselect ();
			b.gameObject.SetActive (false);
		}
		// initialize the button menu based on the abilities of the player's weapon
		for (int i = 0; i < player.stats.weapon.effectorStats.Length; i ++)
		{
			//Debug.Log ("effector # " + i);
			buttons [i].gameObject.SetActive (true);
		}
	}

	// An action button was clicked
	public void ClickedButton(PlayerActionButton b)
	{
		if (b.selected)
			Deselect (b);
		else
			Select (b);
	}

	// Select the action
	public void Select(PlayerActionButton button)
	{
		foreach (PlayerActionButton b in buttons)
		{
			if (b != button)
				b.Deselect ();
		}
		button.Select ();
		player.ChangeActionMode (button.actionModeNumber);
	}

	// Deselect the action (set the action to movement)
	public void Deselect(PlayerActionButton button)
	{
		button.Deselect ();
		player.ChangeActionMode (0);
	}
}
