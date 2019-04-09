using System.Collections;
using System.Collections.Generic;
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



    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    void Update ()
    {
               
        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(0, Input.GetAxis("Mouse X") * MouseSensitivity, 0));
        _rb.MovePosition(transform.position + (transform.forward * Input.GetAxis("Vertical") * MoveSpeed) + (transform.right * Input.GetAxis("Horizontal") * MoveSpeed));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * JumpForce);
            _jump = true;
        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_jump)
        {
            _jump = false;
        }
    }
}

