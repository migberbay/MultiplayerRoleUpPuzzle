using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class NetworkObject : MonoBehaviour
{
    public bool SyncTransform;
    public List<NetworkObject> SyncObjects = new List<NetworkObject>();
    public List<System.Reflection.MethodInfo> methods;
    private int numUnityFunctions = 88;

    private void Start() {
        
    }

    public void GetAllMethodsOfType(Type t){
        var aux = t.GetMethods();
        methods = aux.ToList();
        methods.RemoveRange(aux.Length-numUnityFunctions, numUnityFunctions);
    }
    public void RunInAll(){
        Debug.Log("Test");
    }

    IEnumerator SendSyncObjectsData(){
        while(true){
            yield return new WaitForSeconds(0.2f);
            foreach(var o in SyncObjects){
                
            }
        }
    }

}
