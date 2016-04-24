/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public static class SaveLoad {

	public static PlayerData playerData = new PlayerData ();

	public static void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/gameData.dat");
		bf.Serialize(file, playerData);
		file.Close();
	}

	public static void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/gameData.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);
			playerData = (PlayerData)bf.Deserialize(file);
			file.Close();
		}
	}
}*/