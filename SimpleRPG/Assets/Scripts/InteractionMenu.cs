using UnityEngine;
using System.Collections;

public class InteractionMenu {

    //public Entity e;                        // the entity that is controlled by the player

    /// <summary>
    /// index - 0: up
    /// index - 1: right
    /// index - 2: down
    /// index - 3: left
    /// </summary>
    private Command[] commands = new Command[4];

    /// <summary>
    /// Initializes the position with an interaction
    /// </summary>
    /// <param name="pos">0 - up, 1 - right, 2 - down, 3 - left</param>
    /// <param name="interaction"></param>
    public void SetCommand(int pos, Command interaction)
    {
        commands[pos] = interaction;
    }

	/// <summary>
	/// Executes the command at the position pos and returns if the command 
	/// execution was successful (if command is not null)
	/// </summary>
	/// <param name="pos"></param>
	/// <returns></returns>
    public bool ExecuteCommand(int pos)
    {
		if (commands[pos] != null)
		{
			commands[pos].Execute();
			return true;
		}
		return false;
    }
}
