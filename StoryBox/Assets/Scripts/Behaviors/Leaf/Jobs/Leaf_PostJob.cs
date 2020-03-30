using System.Collections.Generic;
using UnityEngine;

public class Leaf_PostJob : Behavior {

	public const string HAS_JOB_TAG = "has_job";
	public const string JOB_TAG = "job_";
	public const string APPLICANTS_KEY = "_applicants";

	public string jobTitle;
	public int capacity;
	public EntityData data;

	[Header("Write Keys")]
	public string childrenKey;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		// Count children
		List<Entity> children = memory[childrenKey] as List<Entity>;
		int childCount = children != null ? children.Count : 0;

		// Set job availability tags
		string tag = JOB_TAG + jobTitle;
		if (childCount < capacity) {
			if (!entity.tags.Contains(tag))
				entity.tags.Add(tag);
			if (!entity.tags.Contains(HAS_JOB_TAG))
				entity.tags.Add(HAS_JOB_TAG);
		}
		else {
			if (entity.tags.Contains(tag))
				entity.tags.Remove(tag);
			if (entity.tags.Contains(HAS_JOB_TAG))
				entity.tags.Remove(HAS_JOB_TAG);
		}

		// Go thru applicants and process them
		List<Entity> applicants = entity.childEntities.ContainsKey(jobTitle + APPLICANTS_KEY) ? entity.childEntities[jobTitle + APPLICANTS_KEY] : null;
		if (applicants == null || applicants.Count <= 0) {
			return NodeStatus.Success;
		}
		else {
			int i = 0;
			while (i < applicants.Count) {
				Entity applicant = applicants[i];
				if (childCount < capacity) {
					entity.AddChild(childrenKey, applicant);
					applicant.SetParent(entity);
					applicant.memory[Leaf_AcceptJob.GetApplicantStateKey(entity)] = Leaf_AcceptJob.ACCEPTED;
					applicant.TransformTo(data);
					childCount++;
				}
				else {
					applicant.memory[Leaf_AcceptJob.GetApplicantStateKey(entity)] = Leaf_AcceptJob.REJECTED;
				}
				i++;
			}
			applicants.Clear();
		}
		return NodeStatus.Success;
	}
}
