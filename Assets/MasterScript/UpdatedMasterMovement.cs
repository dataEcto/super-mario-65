using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cinemachine;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Audio;

public class UpdatedMasterMovement : MonoBehaviour
{
    //OBJECT variables
    public Transform cam;
    private CharacterController mover;

    //CAMERA Variables
    private Vector3 camF;
    private Vector3 camR;

    //INPUT Variables
    private Vector2 input;
    private float previousInputY;


    //PHYSICS Variables
    private Vector3 intention;
    private Vector3 velocity;
    private Vector3 velocityXZ;
    public float speed;
    public float accel;
    public float turnSpeed;
    public float jumpSpeed;
    //Below, we will lerp turnSpeed with these 2 values
    float turnSpeedLow;
    float turnSpeedHigh;

    //GRAVITY
    public float grav = 10f;
    public bool grounded = false;
    public float maxDistance;

    public static UpdatedMasterMovement Singleton;



    public bool OnSlide = false;

    //Audio
    public AudioSource marioMovement;
    public AudioClip marioWalk;
    public AudioSource marioJump;
    public bool isplayed;
    public AudioSource marioYahoo;
    public AudioSource backGroundMusicSlide;
    public AudioSource Victory;
    public AudioSource itsme;
    public AudioSource chillMusic;
   


    [Header("Results")]
    public float groundSlopeAngle = 0f;            // Angle of the slope in degrees
    public Vector3 groundSlopeDir = Vector3.zero;  // The calculated slope as a vector

    [Header("Settings")]
    public bool showDebug = false;                  // Show debug gizmos and lines
    public LayerMask castingMask;                  // Layer mask for casts. You'll want to ignore the player.
    public float startDistanceFromBottom = 0.2f;   // Should probably be higher than skin width
    public float sphereCastRadius = 0.25f;
    public float sphereCastDistance = 0.75f;       // How far spherecast moves down from origin point

    public float raycastLength = 0.75f;
    public Vector3 rayOriginOffset1 = new Vector3(-0.2f, 0f, 0.16f);
    public Vector3 rayOriginOffset2 = new Vector3(0.2f, 0f, -0.16f);


    /*
    //Camera Stuff
        public enum Movement
        {
            Follow,
            Natural,
            Inverse
        }

        public Movement MovementMode;

        public Camera CamFollow;
        public Camera CamInverse;
        public Camera CamNatural;

    */


    //jump
    public float movementMultiplier;
    public float jumpMultiplier;
    public float fallMultiplier;
    public float walkingMultiplier;
    public float JumpCount;



    //Slide Variables
    //This variable is turned on and runs all of the normal character controlling functions under
    //an if statement
    public bool characterFunctions;
    public Rigidbody marioRB;
    public float slideSpeed;
    
    
    public Animator Anim;




    void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        mover = GetComponent<CharacterController>();
        turnSpeedLow = turnSpeed;
        turnSpeedHigh = turnSpeed * 4;


        //MovementMode = Movement.Inverse;
        characterFunctions = true;

        marioMovement = GetComponent<AudioSource>();
        isplayed = false;
       

    }


    void Update()
    {
        DoInput();
        CalculateCamera();
        CalculateGround();
        DoMove();
        DoGravity();
        if (mover && mover.isGrounded)
        {
            CheckSlope(new Vector3(transform.position.x, transform.position.y - (mover.height / 2) + startDistanceFromBottom, transform.position.z));
        }
        DoSound();

        if (!OnSlide)
        {
            Jumping();

        }
        else
        {
            Vector3 temp = velocity + Quaternion.Euler(0, 0, groundSlopeAngle) * new Vector3(velocity.x, 0, velocity.z);
            velocity = temp.normalized * slideSpeed + Vector3.down * 20;


        }


        //We finally move once DoMove has calculated the velocity, rather than
        //at the same time
        mover.Move(velocity * Time.deltaTime);

        if (velocity.y < 0)
        {
            //player is falling down
            //add to the existing velocity and multiply by gravity and multiply by time since
            velocity = velocity + Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;


        }
        else if (velocity.y > 0)
        {
            //player is jumping up
            velocity = velocity + Vector3.up * Physics.gravity.y * jumpMultiplier * Time.fixedDeltaTime;
        }



        SetAnimation();

        //Debug.Log("The " + MovementMode);
    }




    public void DoInput()
    {

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //Slide Input to prevent upward movement
        if (OnSlide)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), 1);
            Anim.SetBool("Idle", false);
            Debug.Log("SLIDE INPUT");
            //Slide Audio
           


        }

    }

    public void CalculateCamera()
    {
        camF = cam.forward;
        camR = cam.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

    }

    public void CalculateGround()
    {

        Ray playerRay = new Ray(this.transform.position, -Vector3.up);
        RaycastHit hit;
        Debug.DrawRay(playerRay.origin, playerRay.direction * maxDistance, Color.red);
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, maxDistance))
        {
            grounded = true;
            
        }
        else
        {
            grounded = false;
        }


    }

    public void CheckSlope(Vector3 origin)
    {
        RaycastHit hit;
        if (Physics.SphereCast(origin, sphereCastRadius, Vector3.down, out hit, sphereCastDistance, castingMask))
        {
            groundSlopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            Vector3 temp = Vector3.Cross(hit.normal, Vector3.down);
            groundSlopeDir = Vector3.Cross(temp, hit.normal);
        }

        RaycastHit slopeHit1;
        RaycastHit slopeHit2;

        if (Physics.Raycast(origin + rayOriginOffset1, Vector3.down, out slopeHit1, raycastLength))
        {
            if (showDebug)
            {
                Debug.DrawLine(origin + rayOriginOffset1, slopeHit1.point, Color.red);
            }

            // Get angle of slope on hit normal
            float angleOne = Vector3.Angle(slopeHit1.normal, Vector3.up);

            if (Physics.Raycast(origin + rayOriginOffset2, Vector3.down, out slopeHit2, raycastLength))
            {
                // Debug line to second hit point
                if (showDebug)
                {
                    Debug.DrawLine(origin + rayOriginOffset2, slopeHit2.point, Color.red);
                }

                // Get angle of slope of these two hit points.
                float angleTwo = Vector3.Angle(slopeHit2.normal, Vector3.up);
                // 3 collision points: Take the MEDIAN by sorting array and grabbing middle.
                float[] tempArray = { groundSlopeAngle, angleOne, angleTwo };
                Array.Sort(tempArray);
                groundSlopeAngle = tempArray[1];
            }
            else
            {
                // 2 collision points (sphere and first raycast): AVERAGE the two
                float average = (groundSlopeAngle + angleOne) / 2;
                groundSlopeAngle = average;
            }
        }



    }
    //
    //    void OnDrawGizmosSelected()
    //    {
    //        if (showDebug)
    //        {
    //            // Visualize SphereCast with two spheres and a line
    //            Vector3 startPoint = new Vector3(transform.position.x, transform.position.y - (mover.height / 2) + startDistanceFromBottom, transform.position.z);
    //            Vector3 endPoint = new Vector3(transform.position.x, transform.position.y - (mover.height / 2) + startDistanceFromBottom - sphereCastDistance, transform.position.z);
    //
    //            Gizmos.color = Color.white;
    //            Gizmos.DrawWireSphere(startPoint, sphereCastRadius);
    //
    //            Gizmos.color = Color.gray;
    //            Gizmos.DrawWireSphere(endPoint, sphereCastRadius);
    //
    //            Gizmos.DrawLine(startPoint, endPoint);
    //        }
    //    }


    public void DoMove()
    {
        //Relatively move with the cameras directoin
        //(Up and Right)
        Vector3 intention = camF * input.y + camR * input.x;

        float topSpeed = velocity.magnitude / turnSpeed;

        //As Velocity increases, our turn speed should be slower
        //within the range of 0 movement speed to topSpeed
        turnSpeed = Mathf.Lerp(turnSpeedHigh, turnSpeedLow, topSpeed);
        //If there is input...
        if (input.magnitude > 0)
        {
            //....We will get the rotation of the camera, determing the direction we face
            Quaternion rot = Quaternion.LookRotation(intention);
            //And rotate the player in that direction.
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }

        //then, we create a velocity that goes forward, which changes depending on the rotation
        //First, though, we get rid of the Velocity that affects the Y axis
        //Allowing for gravity to be used
        velocityXZ = velocity;
        velocityXZ.y = 0;
        velocityXZ = Vector3.Lerp(velocityXZ, transform.forward * input.magnitude * speed, accel * Time.deltaTime);
        //Now that we made sure everything but the Y is being affected, we finally change the velocity
        //We just use the default velocity.Y as that is being affected by gravity alone
        velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);

    }


    public void DoGravity()
    {

        //First we need to make sure if we are touching
        //the ground first.
        //This makes sure fall speed is consistent
        if (grounded)
        {
            velocity.y = -0.5f;
        }
        else
        {
            //Just changing the velocity to be going downwards.
            velocity.y -= grav * Time.deltaTime;
        }

        //We also set a limit to how long velocity.y can be decreased/increased.
        velocity.y = Mathf.Clamp(velocity.y, -10, 10);
    }

    public void Jumping()
    {
        if (JumpCount >= 1f && grounded == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocity = Vector3.up * movementMultiplier;
                JumpCount = JumpCount - 1;
                marioJump.Play();
                Anim.SetTrigger("Jump");
            }



        }

        if (grounded == true)
        {
            JumpCount = 1;
            
        }

    }

    //Jay stuff
    public void MarioRotation()
    {

        //....We will get the rotation of the camera, determing the direction we face
        Quaternion rot = Quaternion.LookRotation(intention);
        ////And rotate the player in that direction.
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);

    }

    //Jay Stuff that fixed mario spinning when rapidly pressing W
    public void MarioRotationAlternate()
    {



        if (Input.GetKey(KeyCode.A))
        {

            this.transform.Rotate(0, -3, 0);

        }


        if (Input.GetKey(KeyCode.D))
        {

            this.transform.Rotate(0, 3, 0);

        }

    }

    //Made the sound Stuff its own function - Genric
    public void DoSound()
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D)) && grounded == true)
        {
            if (isplayed == true)
                return;
            marioMovement.clip = marioWalk;
            marioMovement.Play();
            isplayed = true;

        }
        else if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) &&
                 !Input.GetKey(KeyCode.D) || !grounded)
        {

            marioMovement.Stop();
            isplayed = false;

        }

        if (OnSlide) {

            isplayed = true;
           }

    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("slide"))
        {
            chillMusic.Stop();
            marioYahoo.Play();
            backGroundMusicSlide.Play();
        }

        if (collision.gameObject.CompareTag("Victory")) 
        {
            backGroundMusicSlide.Stop();
            Victory.Play();
            itsme.Play();
        }

    }


    private void SetAnimation()
    {
        if (OnSlide)
        {
            Anim.SetBool("Slide", true);
            
        }
        else
        {

            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
                 Input.GetKey(KeyCode.D)))
            {
                Anim.SetBool("Run", true);
            }

            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) &&
                !Input.GetKey(KeyCode.D))
            {
                Anim.SetBool("Run", false);

            }
        }

    }
}





