using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;
using UnityEditor;

public class Trigger : MonoBehaviour
{
    public Animator Anim;
    public bool Slide;

    private void OnTriggerExit(Collider other)
    {
        UpdatedMasterMovement.Singleton.OnSlide = Slide;
        Anim.SetBool("Slide", Slide);
    }

}