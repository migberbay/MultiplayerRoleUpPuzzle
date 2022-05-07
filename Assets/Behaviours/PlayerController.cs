using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeedMultiplier = 0.045f;
    public bool isLocalControlled = true;
    public int clientID = 0;
    CharacterController m_charController;
    Vector3 frameMovement = Vector3.zero;


    void Start(){
        m_charController = this.GetComponent<CharacterController>();
        StartCoroutine(TransformSyncer());
    }

    void Update(){
        if(isLocalControlled){MovementInput();}

    }

    void FixedUpdate() {
       if(isLocalControlled){DoMovementLocal();}
       
    }

    IEnumerator TransformSyncer(){
        // send new transform values to server 4 times a second.
        while(true){
            yield return new WaitForSeconds(0.25f);
        }
    }

    void MovementInput(){
        frameMovement = Vector3.zero;
        if(Input.GetKey(KeyCode.W)){
            frameMovement += new Vector3(0,0,1);
        }

        if(Input.GetKey(KeyCode.A)){
            frameMovement += new Vector3(-1,0,0);
        }

        if(Input.GetKey(KeyCode.S)){
            frameMovement += new Vector3(0,0,-1);
        }

        if(Input.GetKey(KeyCode.D)){
            frameMovement += new Vector3(1,0,0);
        }
    }

    void DoMovementLocal(){
        // multiply by factor
        frameMovement *= movementSpeedMultiplier;
        //a apply physics
        frameMovement += Physics.gravity;
        //apply vector movement
        m_charController.Move(frameMovement);
        
        frameMovement.y = 0;
        if(frameMovement != Vector3.zero){
            transform.rotation = Quaternion.LookRotation(frameMovement);
        }
    }
}
