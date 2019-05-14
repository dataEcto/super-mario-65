using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenricCamera : MonoBehaviour
{

    public Transform player;
    private float heading = 0;
    private float tilt = 15;
    public float camDistance = 10;
    public float playerHeight;

    public Vector3 Slope;
    public static GenricCamera Singleton;

    void Start()
    {
        GenricCamera.Singleton = this;
    }


    void LateUpdate()
    {
        heading += Input.GetAxis("Mouse X") * Time.deltaTime * 180;
        tilt += Input.GetAxis("Mouse Y") * Time.deltaTime * 180;

        
        
        
        // Lock Camera Pos 

        if (!UpdatedMasterMovement.Singleton.OnSlide)
        {
            tilt = Mathf.Clamp(tilt,-20,80);
        }
        else
        {
            tilt = Mathf.Clamp(tilt,-5,80);
  //          Vector2 flattenedSlope = new Vector2(Slope.x, Slope.z);

 //           float temp = Vector2.Angle(flattenedSlope, Vector2.up);
            float temp = player.transform.localEulerAngles.y;
            
            heading  = Mathf.Clamp(heading,temp-5,temp+5);
        }
        
        
        
        
        
        transform.rotation = Quaternion.Euler(tilt, heading, 0);

        //How far the camera is from the player
        transform.position = player.position - transform.forward * camDistance + Vector3.up * playerHeight;
        
        

    }
}
