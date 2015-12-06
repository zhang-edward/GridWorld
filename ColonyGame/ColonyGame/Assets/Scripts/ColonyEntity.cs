using UnityEngine;
using System.Collections;

public abstract class ColonyEntity : MovingEntity {

	protected ColonyCenter colonyCenter;
	protected TerritoryMngr territory;

	public void Init(ColonyCenter c, TerritoryMngr t)
	{
		colonyCenter = c;
		territory = t;
	}
}
