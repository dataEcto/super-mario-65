using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;
using UnityEditor;

public class Trigger : MonoBehaviour
{
<<<<<<< HEAD
    public CharacterController PlayerController;
    public MasterMovementEleanorTest MovementScriptEleanorTest;//Replace Genricmovement with MasterMovement
    
    public Animator Anim;
=======
       
    // Camera List
    public GameObject PreviousCamera;
    public GameObject NewCamera;





//    public MasterMovement.Movement LocalMovementMode;


    //public bool LockIntention;
//    private CinemachineBrain brain;

    /*public enum TransitionMode
    {
        Blend,
        CutScene,      
    }
>>>>>>> f718c79580b725770c82ca49c9d031cd9e031b27

    public bool Slide;

    private void OnTriggerExit(Collider other)
    {

        MovementScriptEleanorTest.OnSlide = Slide;
        Anim.SetBool("Slide", Slide);
    }

}