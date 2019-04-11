using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
       
    // Camera List
    public GameObject PreviousCamera;
    public GameObject NewCamera;
    public bool LockIntention;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != GenricMovementElVer.Singleton.gameObject) return;
        PreviousCamera.SetActive(false);
        NewCamera.SetActive(true);
        GenricMovementElVer.Singleton.LockIntention = LockIntention;
        Debug.Log("Enter");

    }
}