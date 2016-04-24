using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class HeroListItem : MonoBehaviour
{
	public bool selected;

	public HeroStats stats;
	public HeroView view;

	public HeroTeamSelector team;

	public void Init(HeroStats stats, HeroTeamSelector team)
	{
		this.stats = stats;
		this.team = team;

		EventTrigger trigger = GetComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerClick;
		entry.callback.AddListener(delegate {team.ButtonClicked(this);});
		trigger.triggers.Add(entry);

		view.Init (stats);
	}

	public void Select()
	{
		selected = true;
		GetComponent<Image> ().color = new Color (.5f, .5f, .5f, 0.5f);
	}

	public void Deselect()
	{
		selected = false;
		GetComponent<Image> ().color = new Color (1, 1, 1, 0.5f);
	}
}

