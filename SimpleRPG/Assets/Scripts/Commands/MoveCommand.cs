using UnityEngine;
using System.Collections;

public class MoveCommand : Command
{
    public int x;
    public int y;

    /// <summary>
    /// Move to a target
    /// </summary>
    /// <param name="e">The entity performing the action</param>
    /// <param name="target">The target of the attack</param>
    /// <param name="damage">Damage to deal to the target</param>
    public MoveCommand(Entity e, int x, int y)
    {
        this.e = e;
        this.x = x;
        this.y = y;
    }

    public override void Execute()
    {
		e.world.AttemptMoveEntity(e, x, y, e.canMove);
		//Debug.Log(this);
		e.done = true;
    }

    public override string ToString()
    {
        return "MoveCommand: " + x + ", " + y;
    }
}
