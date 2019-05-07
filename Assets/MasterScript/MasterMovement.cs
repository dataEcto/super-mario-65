using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Audio;

public class MasterMovement : MonoBehaviour
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
    public Vector3 intention;
    public Vector3 velocity;
    public Vector3 velocityXZ;
    public float speed;
    public float accel;
    public float turnSpeed;
      //Below, we will lerp turnSpeed with these 2 values
    float turnSpeedLow;
    float turnSpeedHigh;
    
    //GRAVITY
    public float grav = 10f;
    public bool grounded = false;
    public float maxDistance;

    public static MasterMovement Singleton;

    public bool LockIntention;
 

    
    //jump
    public float movementMultiplier;
    public float jumpMultiplier;
    public float fallMultiplier;
    public float JumpCount;
    private float MaxJump;
    
    
    //Slide Variables
    //This variable is turned on and runs all of the normal character controlling functions under
    //an if statement
    public bool characterFunctions;
    public GameObject mario;
    public float SlideturnSpeed;
    float SlideturnSpeedLow = 0.5f;
    float SlideturnSpeedHigh = 1.5f;


    //Audio
    public AudioSource marioMovement;
    public AudioClip marioWalk;
    public AudioSource marioJump;
    public bool isplayed;


    
    void Start()
    {
        mover = GetComponent<CharacterController>();
        turnSpeedLow = turnSpeed;
        turnSpeedHigh = turnSpeed * 4;

        SlideturnSpeedLow = SlideturnSpeed;
        SlideturnSpeedHigh = SlideturnSpeed * 2;
        
        
        MaxJump = 1f;
        characterFunctions = true;

        marioMovement = GetComponent<AudioSource>();
        isplayed = false;


    }

    
    void Update()
    {
        DoInput();
        CalculateCamera();
        CalculateGround();
        DoGravity();
        DoSound();
        
        //Character Function allows these to run
        if (characterFunctions)
        {
            DoMove();
            Jumping();
            mover.Move(velocity * Time.deltaTime);
        }
        //Once the player passes by the slide trigger, the rigidbody is activated
        //Thus, we need to switch to a new movement type.
        else
        {
           SlideMovement();
           mover.Move(velocity * Time.deltaTime);
        }
       
        //We finally move once DoMove has calculated the velocity, rather than
        //at the same time
       

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

      
    }

    public void DoInput()
    {
        
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
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

        if (characterFunctions)
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
        
        
       else 
        {
            Ray marioRay = new Ray(mario.transform.position, -Vector3.up);
            RaycastHit marioHit;
            Debug.DrawRay(marioRay.origin, marioRay.direction * maxDistance, Color.blue);
            if (Physics.Raycast(transform.position, -Vector3.up, out marioHit, maxDistance))
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
        }

      
        
    }


    public void DoMove()
    {
        //Relatively move with the cameras directoin
        //(Up and Right)
        intention = camF * input.y + camR * input.x;

        float topSpeed = velocity.magnitude / turnSpeed;

        //As Velocity increases, our turn speed should be slower
        //within the range of 0 movement speed to topSpeed
        turnSpeed = Mathf.Lerp(turnSpeedHigh, turnSpeedLow, topSpeed);
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

    public void SlideMovement()
    {
       
        //marioRB.velocity = new Vector3(direction.x * slideSpeed, marioRB.velocity.y, marioRB.velocity.z); 
        //Set the Z to be relative to Mario's X axis
        
        
        //Relatively move with the cameras directoin
        //(Up and Right)
        Vector3 intention = camF * input.y + camR * input.x;

        float topSpeed = velocity.magnitude / turnSpeed;

        //As Velocity increases, our turn speed should be slower
        //within the range of 0 movement speed to topSpeed
        turnSpeed = Mathf.Lerp(turnSpeedHigh, turnSpeedLow, topSpeed);
        
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
        velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);
        //We just use the default velocity.Y as that is being affected by gravity alone
       
        
        //If I can change the velocity here, it can push mario automatically when it is on the slide.

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
            Debug.Log("Fall Down");
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
            }

           

        }

        if (grounded == true)
        {
            JumpCount = 1;

        }

        if (grounded == false) 
        {
            marioMovement.Stop();
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
        else if (!Input.GetKey(KeyCode.W) &&!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) &&
                 !Input.GetKey(KeyCode.D) || !grounded)
        {


            marioMovement.Stop();
            isplayed = false;




        }





    }
    
}



 

   