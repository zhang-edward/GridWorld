using UnityEngine;
using System.Collections;

[System.Serializable]
public class BaseItem {

	public string itemName;
	//public string itemDescription;

	public BaseItem()
	{}

	public void SetName(string name)
	{
		itemName = name;
	}
}