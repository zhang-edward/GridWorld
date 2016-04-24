using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeroTeamSelector : MonoBehaviour {

	public Transform content;

	public List<HeroListItem> selectedHeroes;
	public List<HeroListItem> heroList;

	public GameObject heroListItemPrefab;

	public void Start()
	{
		Init ();
	}

	public void Init()
	{
		// DEBUG DEBUG DEBUG
		Game.playerData.Init ();
		// DEBUG DEBUG DEBUG

		foreach (HeroStats stat in Game.playerData.players)
		{
			GameObject o = Instantiate (heroListItemPrefab) as GameObject;
			o.transform.SetParent (content, false);
			HeroListItem item = o.GetComponent<HeroListItem> ();
			item.Init (stat, this);
		}
	}

	public void ButtonClicked(HeroListItem item)
	{
		if (item.selected)
			Deselect (item);
		else
			Select (item);
	}

	public void Select(HeroListItem item)
	{
		item.Select ();
		selectedHeroes.Add (item);
		if (selectedHeroes.Count > 4)
			Deselect (selectedHeroes [0]);
			
	}

	public void Deselect(HeroListItem item)
	{
		item.Deselect ();
		selectedHeroes.Remove (item);
	}

	public List<HeroStats> GetTeam()
	{
		List<HeroStats> team = new List<HeroStats> ();
		foreach (HeroListItem item in selectedHeroes)
		{
			team.Add (item.stats);
		}
		return team;
	}
}
