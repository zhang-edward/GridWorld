using UnityEngine;
using System.Collections;

public class Town : Entity {

    //private int Popularity;
    //private int Intimidation;
    public Quest quest;
	public int debugInt;

    void Awake()
    {
		debugInt = Random.Range(0, 1000);
    }

    private void GenerateKillQuest()
    {
        Pawn enemy = world.findEntity<Pawn>();
        if (enemy != null)
            quest = new KillQuest(this, enemy);
        else
            Debug.Log("No enemy found");
    }

	private void GenerateTravelQuest()
	{
		int x = Random.Range(0, World.WORLD_SIZE);
		int y = Random.Range(0, World.WORLD_SIZE);
		// generate new coords if coords is not grass or entity exists on tile
		while (!(world.terrain[y, x] == 1 && !world.EntityExistsAt(x, y)))
		{
			x = Random.Range(0, World.WORLD_SIZE);
			y = Random.Range(0, World.WORLD_SIZE);
		}
		quest = new TravelQuest(this, x, y);
	}

    public override void Act ()
	{
        if (quest == null)
		{
			if (Random.value < 0.5f)
				GenerateKillQuest();
			else
				GenerateTravelQuest();
			if (quest != null)
				quest.setReward(Random.Range(1, 10), Random.Range(50, 99));
		}
        done = true;
	}

	public override string ToString()
	{
		return base.ToString() + debugInt;
	}
}
