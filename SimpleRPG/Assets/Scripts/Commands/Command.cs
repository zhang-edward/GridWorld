using UnityEngine;
using System.Collections;

public abstract class Command
{
    public Entity e;       // the entity that is performing the command

    public abstract void Execute();

    public override string ToString()
    {
        return "Command";
    }
}
