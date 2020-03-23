using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SortingLayerControl : MonoBehaviour {

	public bool updating = false;
	public bool usingCustomPivot = false;
	public float customPivot;
	SpriteRenderer sr;

	void Start() {
		sr = GetComponent<SpriteRenderer>();
		if (updating)
			StartCoroutine(SetSortingOrderRepeating());
		else
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

	private IEnumerator SetSortingOrderRepeating() {
		for (; ; )
		{
			SetSortingOrder();
			yield return null;
		}
	}

	private void SetSortingOrder() {
		float offset;
		if (!usingCustomPivot)
			offset = sr.bounds.size.y / 2;
		else
			offset = -customPivot;

		sr.sortingOrder = (int)((transform.position.y - offset) * -100);
	}
}
