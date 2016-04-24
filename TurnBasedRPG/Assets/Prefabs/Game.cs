using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public class Game : MonoBehaviour {

	public static Game instance;
	public static PlayerData playerData = new PlayerData ();

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (this.gameObject);
		DontDestroyOnLoad (this.gameObject);
	}

	public void Play()
	{
		SceneManager.LoadScene ("Game");
	}

	public static void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/gameData.dat");
		bf.Serialize(file, Game.playerData);
		file.Close();
	}

	public static void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/gameData.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gameData.dat", FileMode.Open);
			Game.playerData = (PlayerData)bf.Deserialize(file);
			file.Close();
		}
	}
}