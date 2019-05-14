using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;
using UnityEditor;

public class Trigger : MonoBehaviour
{
    public bool Slide;
    public bool ResetPosition;

    private void OnTriggerExit(Collider other)
    {
        UpdatedMasterMovement.Singleton.OnSlide = Slide;
        UpdatedMasterMovement.Singleton.Anim.SetBool("Slide", Slide);
        if (ResetPosition) UpdatedMasterMovement.Singleton.Reset();
        Debug.Log(name);
    }

}