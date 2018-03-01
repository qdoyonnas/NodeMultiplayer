using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    string gameDataFilePath = "/StreamingAssets/data.json";

	// Use this for initialization
	void Start ()
    {
        GameData jsonObj = new GameData();
        jsonObj.playerName = "Default";
        jsonObj.score = 100;
        jsonObj.timePlayed = 600.5f;
        jsonObj.lastLogin = "March 1, 2018";

        string json = JsonUtility.ToJson(jsonObj);
        string filePath = Application.dataPath + gameDataFilePath;
        File.WriteAllText(filePath, json);
	}
}
