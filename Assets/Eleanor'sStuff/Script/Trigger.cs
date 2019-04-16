using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;

public class Trigger : MonoBehaviour
{
       
    // Camera List
    public GameObject PreviousCamera;
    public GameObject NewCamera;
    public bool LockIntention;
    private CinemachineBrain brain;

    /*public enum TransitionMode
    {
        Blend,
        CutScene,      
    }

    public TransitionMode Transition;

    private void Start()
    {
        brain = FindObjectOfType<CinemachineBrain>();
        switch (Transition)
        {
            case TransitionMode.Blend:
                brain.m_DefaultBlend.m_Time = 2;
                break;
            case TransitionMode.CutScene:
                brain.m_DefaultBlend.m_Time
                    = brain.CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);= 0;
                break;
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != GenricMovementElVer.Singleton.gameObject) return;
        PreviousCamera.SetActive(false);
        NewCamera.SetActive(true);
        GenricMovementElVer.Singleton.LockIntention = LockIntention;
        Debug.Log("Enter");

    }
    
}