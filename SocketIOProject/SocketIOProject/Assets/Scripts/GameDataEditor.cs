using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SocketIO;

public class GameDataEditor : EditorWindow
{
    string gameDataFilePath = "/StreamingAssets/data.json";
    public GameData editorData;

    [MenuItem("Window/Game Data Editor")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(GameDataEditor)).Show();
    }

    void OnGUI()
    {
        if (editorData != null) {
            SerializedObject serializedObj = new SerializedObject(this);
            SerializedProperty serializedProp = serializedObj.FindProperty("editorData");
            EditorGUILayout.PropertyField(serializedProp, true);
            serializedObj.ApplyModifiedProperties();

            if (GUILayout.Button("Save Game Data")) {
                SaveGameData();
            }
        }

        if( GUILayout.Button("Load Game Data") ) {
            LoadGameData();
        }
    }

    void LoadGameData()
    {
        string filePath = Application.dataPath + gameDataFilePath;

        if ( File.Exists(filePath) ) {
            string gameData = File.ReadAllText(filePath);
            editorData = JsonUtility.FromJson<GameData>(gameData);
        } else {
            editorData = new GameData();
        }
    }

    void SaveGameData()
    {
        string jsonObj = JsonUtility.ToJson(editorData);
        string filePath = Application.dataPath + gameDataFilePath;
        File.WriteAllText(filePath, jsonObj);

        SendGameData();
    }

    void SendGameData()
    {
        GameObject server;
        SocketIOComponent socket;

        server = GameObject.Find("Server");
        if (server == null)
        {
            throw new System.Exception("Server Object not found!");
        }
        socket = server.GetComponent<SocketIOComponent>();
        if (socket == null)
        {
            throw new System.Exception("Socket component not found!");
        }

        string jsonObj = JsonUtility.ToJson(editorData);
        socket.Emit("sent gameData", new JSONObject(jsonObj));
    }
}
