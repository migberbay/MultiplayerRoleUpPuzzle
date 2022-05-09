using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMultiplayerManager : MonoBehaviour
{
    public List<NetworkObject> SyncObjects = new List<NetworkObject>();

    void Start()
    {
        // find all network objects in scene and add them to the syncObject list
        SyncObjects.AddRange(FindObjectsOfType<NetworkObject>());
    }

    IEnumerator SendSyncObjectsData(){
        while(true){
            yield return new WaitForSeconds(0.2f);
            foreach(var o in SyncObjects){
                
            }
        }
    }
    
}
