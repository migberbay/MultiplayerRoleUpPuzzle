using UnityEngine;

[RequireComponent(typeof(Server))]
[RequireComponent(typeof(Client))]
public class ConnectionManager : MonoBehaviour
{
    public bool isHost = false;
    public int port = 42069, clientID = 0;
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
        // TODO: send a message to all clients
    }

    //Client Only Functions:
    public void SendMessageToServer(string message){
        if(isHost){return;}
        client.SendMessageToServer(message);
    }

    //Server Only Functions:
    public void SendMessageToClient(int clientID, string message){
        if(!isHost){return;}
        server.SendMessageToClient(clientID, message);
    }

    public void BroadcastClients(string message){
        if(!isHost){return;}
        var n = server.connectedTcpClients.Count;
        for (int i = 1; i <= n; i++)
        { 
            server.SendMessageToClient(i, message);
        }
    }

    //
    void OpenPort(){
        // Try to UPnP the port that was indicated.
    }

    void OnApplicationQuit() {
        if(isHost){
            StopServer();
        }
        else{

        }
    }
}
