using System;
using System.Collections; 
using System.Collections.Generic; 
using System.Net; 
using System.Net.Sockets; 
using System.Text; 
using System.Threading; 
using UnityEngine;  

public class Server : MonoBehaviour {  	
	#region key variables 	
	/// <summary> 	
	/// TCPListener to listen for incomming TCP connection requests. 	
	/// </summary> 	
	private TcpListener tcpListener; 

	/// <summary> 
	/// Background thread for TcpServer workload. 	
	/// </summary> 	
	private Thread tcpListenerThread;  	

	/// <summary> 	
	/// Dictionary of TCP clients matched to their ids.
	/// </summary> 	
	public Dictionary<int, TcpClient> connectedTcpClients;

	/// <summary> 	
	/// Threads to handle the different client messages matched to the client id.
	/// </summary> 	
	public Dictionary<int, Thread> clientThreads;

	#endregion 	
		
	// Default port, next unasigned client index
    private int port = 42069, lastClientIndex = 1;
	
	public void Initialize(int p) { 
		connectedTcpClients = new Dictionary<int, TcpClient>();
		clientThreads = new Dictionary<int, Thread>();
		// Start TcpServer background thread 		
		tcpListenerThread = new Thread (new ThreadStart(ListenForIncommingConnectionRequests)); 		
		tcpListenerThread.IsBackground = true; 	
        port = p;
		tcpListenerThread.Start();
	}  	
	
	// void Update() { 		
	// 	if (Input.GetKeyDown(KeyCode.Space)) {             
	// 		SendMessageToClient(1, "Server Test Message"); 
	// 		SendMessageToClient(2, "Server Test Message");  
	// 	} 	
	// }  	
	
	/// <summary> 	
	/// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
	/// </summary> 	
	private void ListenForIncommingConnectionRequests () { 		
		try { 			
			// Create listener on localhost port and indicated port			
			tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), port); 			
			tcpListener.Start();              
			Debug.Log("Server is listening");          			
			while (true) { 		
				connectedTcpClients[lastClientIndex] = tcpListener.AcceptTcpClient();	
				var clientListenThread = new Thread (new ThreadStart(HandleMessagesFromClient)); 		
				clientListenThread.IsBackground = true; 	
				clientListenThread.Start();
				clientThreads[lastClientIndex] = clientListenThread;
				Debug.Log($"New client registered with UID: {lastClientIndex}");
				lastClientIndex++;
			} 		
		} 		
		catch (SocketException socketException) { 			
			Debug.Log("SocketException " + socketException.ToString()); 		
		}     
	}  	

	/// <summary> 	
	/// Handles incoming messages from the corresponding client
	/// </summary> 	
	private void HandleMessagesFromClient(){ 					
		var clientID = lastClientIndex;
		Byte[] bytes = new Byte[1024];
		while(true){  
			using (NetworkStream stream = connectedTcpClients[clientID].GetStream()) {
				int length;
				// Read incomming stream into byte arrary.
				while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) {
					var incommingData = new byte[length];
					Array.Copy(bytes, 0, incommingData, 0, length);
					// Convert byte array to string message.
					string clientMessage = Encoding.ASCII.GetString(incommingData);
					Debug.Log($"recieved message from client {clientID}:" + clientMessage);
				}
			}
		} 				
	} 

	/// <summary> 	
	/// Send message to client using socket connection. 	
	/// </summary> 	
	public void SendMessageToClient(int id, string serverMessage) { 		
		if (connectedTcpClients[id] == null) {             
			return;  
		}  		
		var connectedTcpClient = connectedTcpClients[id];
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = connectedTcpClient.GetStream(); 			
			if (stream.CanWrite) {                  			
				// Convert string message to byte array.                 
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage); 				
				// Write byte array to socketConnection stream.               
				stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);               
				Debug.Log("Server sent his message - should be received by client");          
			}       
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		} 	
	} 
}