using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SortingLayerControl : MonoBehaviour {

	public bool updating = false;
	public bool usingCustomPivot = false;
	public float customPivot;
	SpriteRenderer sr;

	void OnEnable() {
		sr = GetComponent<SpriteRenderer>();
		if (!updating)
			SetSortingOrder();
	}

	private void OnDrawGizmosSelected() {
		if (usingCustomPivot) {
			Vector3 customPivotPosition = transform.position + (Vector3.up * customPivot);
			Gizmos.DrawWireSphere(customPivotPosition, 0.2f);
		}
		else {
			float offset = sr.bounds.size.y / 2;
			Vector3 customPivotPosition = transform.position + (Vector3.down * offset);
			Gizmos.DrawWireSphere(customPivotPosition, 0.2f);
		}
	}

	void Update() {
		if (updating)
			SetSortingOrder();
	}

	private void SetSortingOrder() {
		float offset;
		if (!usingCustomPivot)
			offset = 0;
		else
			offset = -customPivot;

		sr.sortingOrder = (int)((transform.position.y - offset) * -100);
	}
}
