using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
	public List<HeroStats> players;

	public void Init()
	{
		players = new List<HeroStats> ();
		players.Add (HeroStats.DebugMakeNewPlayerStats ());
		players.Add (HeroStats.DebugMakeNewPlayerStats ());
		players.Add (HeroStats.DebugMakeNewPlayerStats ());
		players.Add (HeroStats.DebugMakeNewPlayerStats ());
		players.Add (HeroStats.DebugMakeNewPlayerStats ());
		players.Add (HeroStats.DebugMakeNewPlayerStats ());
	}
}

