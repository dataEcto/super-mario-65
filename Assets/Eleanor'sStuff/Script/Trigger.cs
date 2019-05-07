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





//    public MasterMovement.Movement LocalMovementMode;


    //public bool LockIntention;
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
        //Change the name of this singleton to what script is controlling marios movement in whatever scene it is
        //ex) Genric Test Scene's Mario has Genricmovment. Change the if to have GenricMovement.singleton
        if (other.gameObject !=  MasterMovement.Singleton.gameObject  ) return;
        PreviousCamera.SetActive(false);
        NewCamera.SetActive(true);
       // GenricMovementElVer.Singleton.LockIntention = LockIntention;

       //GenricMovement.Singleton.LockIntention = LockIntention;
       // MasterMovement.Singleton.LockIntention = LockIntention;

       // GenricMovement.Singleton.LockIntention = LockIntention;
       // MasterMovement.Singleton.MovementMode = LocalMovementMode;

        Debug.Log("Enter");

    }
    
}