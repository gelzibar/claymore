using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HillNetworkManagerHUD : NetworkBehaviour {

	private NetworkManager myNetworkManager;
	private NetworkClient myNetworkClient;
	public InputField ipAddress;

	void Awake() {
		myNetworkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager> ();
		myNetworkClient = new NetworkClient ();
		ipAddress = ipAddress.GetComponent<InputField> ();
		ipAddress.text = "127.0.0.1";
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void StartServer() {
		if (!NetworkClient.active && !NetworkServer.active) {
			Debug.Log ("Starting Server");
			Debug.Log ("Listening Port: " + myNetworkManager.networkPort);
			myNetworkManager.StartHost ();
		}
	}

	public void SetupLocalClient()
	{
		myNetworkClient.RegisterHandler(MsgType.Connect, OnConnected);
		myNetworkManager.networkAddress = ipAddress.text;
		myNetworkManager.StartClient ();
	}

	public virtual void OnClientConnect(NetworkConnection conn)
	{

	}

	// client function
	public void OnConnected(NetworkMessage netMsg)
	{
		Debug.Log("Connected to server");
	}
}
