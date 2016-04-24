using UnityEngine;
using System.Collections;

public abstract class ColonyBuilding : Entity {
	
	protected ColonyCenter colonyCenter;
	protected TerritoryMngr territory;
	
	public void Init(ColonyCenter c, TerritoryMngr t)
	{
		colonyCenter = c;
		territory = t;
	}
}
