using UnityEngine;
using System.Collections.Generic;

public class Leaf_EntityForEach : Behavior {

	public Behavior condition;

	[Header("Read Keys")]
	public string entitiesKeyIn;

	[Header("Entity to Pass to condition")]
	public string entityConditionKey;

	[Header("Write Keys")]
	public string entitiesKeyOut;
	public string countKey;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		memory.SetDefault(entitiesKeyIn, new List<Entity>());
		List<Entity> entities = (List<Entity>)memory[entitiesKeyIn];
		List<Entity> filteredEntities = new List<Entity>();

		foreach (Entity e in entities) {
			memory[entityConditionKey] = e;
			switch (condition.ExecuteAction(entity, memory)) {
				case NodeStatus.Success:
					filteredEntities.Add(e);
					break;
				case NodeStatus.Failure:
					break;
			}
		}
		// Cleanup
		memory.RemoveKey(entityConditionKey);

		// Write output
		memory[entitiesKeyOut] = filteredEntities;
		memory[countKey] = filteredEntities.Count;
		return NodeStatus.Success;

	}

}
