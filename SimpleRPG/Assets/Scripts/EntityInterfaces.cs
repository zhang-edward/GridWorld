using UnityEngine;
using System.Collections;

public interface ICombatable {

	// return if object dies
	bool Damage(int amt);
	void Heal(int amt);
    bool Alive();
}

public interface IControllable
{
	bool Controllable();
	void InitControl();
	void ReleaseControl();
	void ExecuteCommand(int pos);
	void InitCommands();
	Entity GetEntity();
}