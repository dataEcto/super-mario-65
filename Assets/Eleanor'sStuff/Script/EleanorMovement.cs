using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class EleanorMovement : MonoBehaviour
{

    private Rigidbody _rb;
    private bool _walk;
    private bool _walkAudioPlaying;
    private bool _jump;
    private bool _end;
       
    
    public float MouseSensitivity;
    public float MoveSpeed;
    public float JumpForce;

    public CinemachineVirtualCamera cam;

    private Vector3 camF;
    private Vector3 camR;



    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    void LateUpdate ()
    {
//        camF = cam.forward;
//        camR = cam.right;
//
//        camF.y = 0;
//        camR.y = 0;
//        camF = camF.normalized;
//        camR = camR.normalized;
//               
//        _rb.transform.LookAt(FollowCamera.transform.position + FollowCamera.transform.forward * 5);
//        _rb.MovePosition(transform.position + (transform.forward * Input.GetAxis("Vertical") * MoveSpeed) + (transform.right * Input.GetAxis("Horizontal") * MoveSpeed));
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            _rb.AddForce(Vector3.up * JumpForce);
//            _jump = true;
//        }
//       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_jump && _rb.velocity.y < 0.001f)
        {
            _jump = false;
        }
    }
}

