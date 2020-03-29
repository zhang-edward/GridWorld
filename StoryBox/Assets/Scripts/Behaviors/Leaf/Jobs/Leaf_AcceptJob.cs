using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf_AcceptJob : Behavior
{
	public const int NO_STATUS = 0;
	public const int APPLIED = 1;
	public const int REJECTED = 2;
	public const int ACCEPTED = 3;

	[Header("Read Keys")]
	[Tooltip("Entity")]
	public string employerKey;

	protected override NodeStatus Act(Entity entity, Memory memory) {
		Entity employer = memory[employerKey] as Entity;
		string applicationStateKey = GetApplicantStateKey(employer);
		memory.SetDefault(applicationStateKey, NO_STATUS);
		int status = (int)memory[applicationStateKey];

		switch (status) {
			// Apply to job
			case NO_STATUS:
				foreach (string tag in employer.tags) {
					if (tag.StartsWith(Leaf_PostJob.JOB_TAG)) {
						string positionName = tag.Substring(Leaf_PostJob.JOB_TAG.Length);
						employer.AddChild(positionName + Leaf_PostJob.APPLICANTS_TAG, entity);
						memory[applicationStateKey] = true;
						return NodeStatus.Running;
					}
				}
				return NodeStatus.Failure;
			// Application in process by Leaf_PostJob
			case APPLIED:
				return NodeStatus.Running;
			// Accepted
			case ACCEPTED:
				memory[GetApplicantStateKey(employer)] = NO_STATUS;
				return NodeStatus.Success;
			// Rejected
			case REJECTED:
				memory[applicationStateKey] = NO_STATUS;
				return NodeStatus.Failure;
			// This should never run
			default:
				Debug.LogError($"Invalid state for {applicationStateKey}");
				memory[applicationStateKey] = NO_STATUS;
				return NodeStatus.Failure;
		}

	}

	public static string GetApplicantStateKey(Entity employer) {
		return "applied_" + employer.gameObject.GetInstanceID();
	}
}
