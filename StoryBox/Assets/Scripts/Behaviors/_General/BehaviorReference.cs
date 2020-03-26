using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorReference : Behavior {
    public Behavior behavior;
	protected override NodeStatus Act(Entity entity, Memory memory) {
		return behavior.ExecuteAction(entity, memory);
	}
}
