using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class GenricCamera : MonoBehaviour
{

    public Transform player;
    private float heading = 0;
    private float tilt = 15;
    public float camDistance = 10;
    public float playerHeight;

    void Start()
    {
        
    }


    void LateUpdate()
    {
        heading += Input.GetAxis("Mouse X") * Time.deltaTime * 180;
        tilt += Input.GetAxis("Mouse Y") * Time.deltaTime * 180;

        tilt = Mathf.Clamp(tilt,-80,80);
        
        transform.rotation = Quaternion.Euler(tilt, heading, 0);

        //How far the camera is from the player
        transform.position = player.position - transform.forward * camDistance + Vector3.up * playerHeight;
        
        
        // Lock Camera Pos 
    }
}
