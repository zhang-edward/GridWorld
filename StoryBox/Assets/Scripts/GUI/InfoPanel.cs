using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour {

	public GameObject trackerPrefabs;
	public TMP_Text entityNameText;
	public TMP_Text healthText;

	private Entity entity;
	private InfoPanelData data;
	private List<MemoryIntTracker> memoryIntTrackers = new List<MemoryIntTracker>();
	private Vector2Int lastClicked = new Vector2Int(-1, -1);
	private int entityIndex;

	public void Init(Entity entity) {
		this.entity = entity;
		this.data = entity.infoPanelData;

		foreach (MemoryIntTracker tracker in memoryIntTrackers) {
			Destroy(tracker.gameObject);
		}
		memoryIntTrackers.Clear();
		if (data != null) {
			foreach (string key in data.displayedMemoryIntKeys) {
				GameObject obj = Instantiate(trackerPrefabs, this.transform);
				MemoryIntTracker tracker = obj.GetComponent<MemoryIntTracker>();
				tracker.Init(entity.memory, key);
				memoryIntTrackers.Add(tracker);
			}
		}
		Tick();
	}

	void Start() {
		GameManager.instance.onTick += Tick;
	}

	void Update() {
		Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2Int coords = new Vector2Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
		if (Input.GetMouseButtonDown(0)) {
			List<Entity> l = EntityManager.instance.GetEntitiesAt(coords.x, coords.y);
			if (coords != lastClicked)
				entityIndex = 0;
			if (l != null && l.Count > 0) {
				Init(l[entityIndex]);
				entityIndex = (entityIndex + 1) % l.Count;
				lastClicked = coords;
			}
		}
	}

	public void Tick() {
		if (entity == null)
			return;
		entityNameText.text = entity.gameObject.name;
		healthText.text = $"Health: {entity.health}/{entity.maxHealth}";
		foreach (MemoryIntTracker tracker in memoryIntTrackers)
			tracker.Tick();
	}
}