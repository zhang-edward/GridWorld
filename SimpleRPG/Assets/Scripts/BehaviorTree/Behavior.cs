using UnityEngine;
using System.Collections;

namespace Behaviors
{
	public abstract class Behavior
	{

		//protected NodeStatus status;

		public Behavior() { }

		public abstract void Reset();
		public abstract NodeStatus Act();
	}
}