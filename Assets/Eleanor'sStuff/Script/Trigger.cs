using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;

public class Trigger : MonoBehaviour
{
    public CharacterController PlayerController;
    public MasterMovementEleanorTest MovementScriptEleanorTest;//Replace Genricmovement with MasterMovement
    
    public Animator Anim;

    public bool Slide;

    private void OnTriggerExit(Collider other)
    {

        MovementScriptEleanorTest.OnSlide = Slide;
        Anim.SetBool("Slide", Slide);
    }

}