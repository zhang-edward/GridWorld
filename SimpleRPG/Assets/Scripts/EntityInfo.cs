using UnityEngine;
using System.Collections;

public class EntityInfo : MonoBehaviour {

	private EntityInfoPanel PlayerInfo;
	private EntityInfoPanel inspectedInfo;

	void Awake()
	{
		PlayerInfo = transform.FindChild("PlayerInfo").GetComponent<EntityInfoPanel>();
		inspectedInfo = transform.FindChild("InspectedInfo").GetComponent<EntityInfoPanel>();

		PlayerInfo.gameObject.SetActive(false);
		inspectedInfo.gameObject.SetActive(false);
	}

	void Start()
	{
		Player.instance.OnChangeEntityControl.AddListener(InitPlayerInfoPanel);
	}

	private void InitPlayerInfoPanel()
	{
		// get a living entity from the player controlled entity
		Entity e = Player.instance.controlledEntity.GetEntity();
		LivingEntity livingEntity = e as LivingEntity;

		if (livingEntity != null)
		{
			PlayerInfo.gameObject.SetActive(true);
			PlayerInfo.Init(livingEntity);
		}
		else
		{
			Debug.LogError("Player cannot control non-living entity");
		}
	}

	public void InitInspection(LivingEntity e)
	{
		inspectedInfo.gameObject.SetActive(true);
		inspectedInfo.Init(e);
	}
}
