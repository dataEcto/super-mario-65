using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCCTrigger : MonoBehaviour
{
    
    //This script is going to disable the Character Controller component of Mario
    //Along side that, we will enable the players rigidbody as well.
    //The goal is to get it sliding down the ramp with the rigidbody
    //But also not have to deal with weird rigidbody motions when the player isn't on the slide.
    
    //Id also need to disable the rotation stuff thats going on in the mastermovement

    
    public CharacterController PlayerController;
    public Rigidbody PlayerRB;
    public MasterMovement movementScript;//Replace Genricmovement with MasterMovement
    public MasterMovementEleanorTest MovementScriptEleanorTest;//Replace Genricmovement with MasterMovement

    void Start()
    {
       
    }

    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
        ///Enable the Character Controller's Rigidbody
        //PlayerController.enabled = false;
//        PlayerRB.isKinematic = false;
//        movementScript.characterFunctions = false;
        MovementScriptEleanorTest.OnSlide = true;
        Debug.Log("Disable The Character Controller");
    }
}
