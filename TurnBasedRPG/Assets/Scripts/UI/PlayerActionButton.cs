using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerActionButton : MonoBehaviour {

	public PlayerActionMenu menu;
	public int actionModeNumber;
	public bool selected;

	public void Init(PlayerActionMenu menu, int num)
	{
		EventTrigger trigger = GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener(delegate {menu.ClickedButton(this);});
		trigger.triggers.Add(entry);

		this.menu = menu;
		actionModeNumber = num;
	}

	public void Select()
	{
		selected = true;
		GetComponent<Image> ().color = new Color (1, 0, 0);
	}

	public void Deselect()
	{
		selected = false;
		GetComponent<Image> ().color = new Color (1, 1, 1);
	}
}
