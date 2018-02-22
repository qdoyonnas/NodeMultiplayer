using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkMovement : MonoBehaviour
{
    Vector3 position;
    
	void Start ()
    {
		
	}
	
	void FixedUpdate ()
    {
        position = transform.position;
        OnMove(position);
	}

    public void OnMove(Vector3 pos)
    {
        string jsonObject = VectorToJson(pos);
        //Debug.Log("Sending position: " + jsonObject);
        Network.socket.Emit("move", new JSONObject(jsonObject));
    }

    string VectorToJson(Vector3 vector)
    {
        string str = string.Format(@"{{""x"":""{0}"", ""z"":""{1}""}}", vector.x, vector.z);

        return str;
    }
}
