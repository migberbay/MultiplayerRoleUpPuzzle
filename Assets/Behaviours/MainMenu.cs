using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Button createGameHostButton, joinGamePlayerButton;
    public TMP_InputField portInputFieldHost, portInputFieldJClient, IPInputFieldClient, clientUsernameInput, hostUsernameInput;
    public TMP_Text errorMessageText;
    public ConnectionManager connManager;

    public void JoinButtonAction(){
        var ip_text = IPInputFieldClient.text;
        var port_text = portInputFieldJClient.text;
        if(ValidateIP(ip_text) && ValidatePort(port_text)){
            connManager.StartAsClient(port_text, ip_text);
            SceneManager.LoadScene("MainScene");
        }else{StartCoroutine(clearErrorAfterSeconds(4));}
    }

    public void CreateButtonAction(){
        var port_text = portInputFieldHost.text;
        if(ValidatePort(port_text)){
            connManager.StartAsServer(port_text);
            SceneManager.LoadScene("MainScene");
            // Start a client in localhost after the scene is loaded.
            connManager.StartAsClient(port_text, "127.0.0.1");
        }else{StartCoroutine(clearErrorAfterSeconds(4));}
    }

    public IEnumerator clearErrorAfterSeconds(float seconds){
        yield return new WaitForSeconds(seconds);
        errorMessageText.text = "";
    }

    bool ValidateIP(string ip){
        Regex re = new Regex(@"^([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])\.([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])\.([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])\.([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$");
        var res = re.IsMatch(ip);
        if(!res){
            errorMessageText.text = "That IP is Invalid";   
        }
        return res;
    }

    bool ValidatePort(string port){
        try{
            Debug.Log(port);
            var p = int.Parse(port);
            if(p >= 1000 && p <= 65535){
                return true;
            }else{
                errorMessageText.text = "Port must be: 1000-65535";
                return false;
            }
        }catch(Exception e){
            errorMessageText.text = "Port must be a whole number";
            Debug.Log(e.ToString());   
            return false;
        } 
    }

    private void changeSceneAs(string actor){
        if(actor == "host"){

        }

        if(actor == "client"){


        }
    }
}
