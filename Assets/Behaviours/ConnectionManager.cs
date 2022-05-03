using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Net.Sockets;

public class ConnectionManager : MonoBehaviour
{
    public bool isHost = false;
    public int port = 42069, clientID=0;
    public string ip = "localhost", username = "";
    
    Server server;
    Client client;

    private void Start() {
        server = this.GetComponent<Server>();
        client = this.GetComponent<Client>();
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartAsServer(string portStr){
        // start as a server and join the main scene with the selected parameters.
        port = int.Parse(portStr);
        server.enabled = true;
        OpenPort();
        isHost = true;
        server.Initialize(port);
    }

    public void StartAsClient(string portStr, string ipStr){
        // try to connect to the server if it fails prompt an error message
        port = int.Parse(portStr);
        client.enabled = true;
        client.Initialize(ip, port);
    }

    public void StopServer(){
        // well, that...
    }

    public void SendMessageToServer(string message){
        if(isHost){return;}
        client.SendMessageToServer(message);
    }

    public void SendMessageToClients(string message, int clientID){
        if(isHost){return;}
        
    }

    void OpenPort(){
        // Try to UPnP the port tht was indicated.
    }

    void OnApplicationQuit() {
        StopServer();
    }
}
