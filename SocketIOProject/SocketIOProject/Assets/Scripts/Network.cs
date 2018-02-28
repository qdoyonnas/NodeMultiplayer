using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour
{
	static public SocketIOComponent socket;
	public GameObject playerPrefab;

	Dictionary<string, GameObject> players;

	// Use this for initialization
	void Start ()
    {
		socket = GetComponent<SocketIOComponent> ();
		socket.On ("open", OnConnected);
		socket.On ("spawn player", OnSpawned);
		socket.On ("disconnected", OnDisconnected);
        socket.On("move", OnMove);
		players = new Dictionary<string, GameObject> ();
	}
	
	// Tells us we are connected
	void OnConnected (SocketIOEvent e)
    {
		Debug.Log ("We are Connected");
		socket.Emit ("playerhere");
	}

	void OnSpawned(SocketIOEvent e)
    {
		Debug.Log ("Player Spawned!" + e.data);
        string id = e.data["id"].ToString();

		var player = Instantiate (playerPrefab);
        player.name = "player_" + id;
		players.Add (id, player);
		Debug.Log ("count " + players.Count);
	}

	void OnDisconnected(SocketIOEvent e)
    {
		Debug.Log ("player disconnected: " + e.data);

		var id = e.data ["id"].ToString ();

		var player = players [id];
		Destroy (player);
		players.Remove (id);
	}

    void OnMove(SocketIOEvent e)
    {
        string id = e.data["id"].ToString();

        GameObject player = players[id];

        CharacterMovement moveScript = player.GetComponent<CharacterMovement>();
        Vector3 pos = new Vector3(GetFloatFromJson(e.data["pos"], "x"), 0.0f, GetFloatFromJson(e.data["pos"], "z"));
        float h = GetFloatFromJson(e.data["vel"], "x");
        float v = GetFloatFromJson(e.data["vel"], "z");
        moveScript.NetworkMovement(h, v, pos);
    }

    float GetFloatFromJson(JSONObject data, string key)
    {
        return float.Parse(data[key].ToString().Replace("\"", string.Empty));
    }
}
