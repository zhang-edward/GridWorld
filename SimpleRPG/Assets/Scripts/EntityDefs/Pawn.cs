using UnityEngine;
using System.Collections;

public class Pawn : LivingEntity, IControllable
{
    private bool controlled;

    public override void Act()
    {
        // set default commands
        defaultCommands[0] = new MoveCommand(this, x, y + 1);
        defaultCommands[1] = new MoveCommand(this, x + 1, y);
        defaultCommands[2] = new MoveCommand(this, x, y - 1);
        defaultCommands[3] = new MoveCommand(this, x - 1, y);
        // initialize special interactions if entities are nearby
        InitCommands();

        // if this entity is not controlled, execute random action
        if (!controlled)
        {
			/* int pos = Random.Range(0, 4);
			 ExecuteCommand(pos);*/
			done = true;
        }
    }

	/// <summary>
	/// Create possible Commands for this entity to perform
	/// </summary>
	/// <summary>
	/// Create possible Commands for this entity to perform
	/// </summary>
	public void InitCommands()
	{
		Entity[] neighbors = world.getAdjacentEntities(x, y);
		for (int i = 0; i < 4; i++)
		{
			// reset the command at this position
			interactions[i] = null;

			// find the entity at the position
			Entity e = neighbors[i];
			if (e != null)
			{
				// ENEMY INTERACTION
				if (e is Pawn)
				{
					Pawn enemy = e as Pawn;
					if (enemy.faction != this.faction)
						interactions[i] = new BattleCommand(this, e as Pawn);
				}
				// HERO INTERACTION
				else if (e is Hero)
				{
					Hero hero = e as Hero;
					if (hero.faction != this.faction)
						interactions[i] = new BattleCommand(this, e as Hero);
				}
			}
			else
			{
				interactions[i] = null;
			}
		}
	}


	public bool Controllable()
    {
        return Alive();
    }

    public void InitControl()
    {
        controlled = true;
    }

    public void ReleaseControl()
    {
        controlled = false;
        done = true;
    }
}
