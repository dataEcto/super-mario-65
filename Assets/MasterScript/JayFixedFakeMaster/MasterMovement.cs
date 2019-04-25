using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

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

    public static MasterMovement Singleton;

    public bool LockIntention;

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
    
    
    //audio
    public AudioSource walkingSound;
    
    //jump
    public float movementMultiplier;
    public float jumpMultiplier;
    public float fallMultiplier;
    public float walkingMultiplier;
    public float JumpCount;
    private float MaxJump;



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
        
        MaxJump = 1f;
        MovementMode = Movement.Inverse;
    }

    
    void Update()
    {
        DoInput();
        CalculateCamera();
        CalculateGround();
        DoMove();
        DoGravity();
        Jumping();
      
        //We finally move once DoMove has calculated the velocity, rather than
        //at the same time
        mover.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D))
        {


            walkingSound.Play();


        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) ||
                 Input.GetKeyUp(KeyCode.D))
        {


           

            walkingSound.Stop();
   

        }
        
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



        Debug.Log("The " + MovementMode);


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

        Ray playerRay = new Ray(this.transform.position, -Vector3.up);
        RaycastHit hit;
        Debug.DrawRay(playerRay.origin, playerRay.direction * maxDistance, Color.green);
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, maxDistance))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
        
        
    }

    //Jay Added some stuff here lol
    public void DoMove()
    {
        //Relatively move with the cameras directoin
        //(Up and Right)
        if (MovementMode == Movement.Follow)
        {
            intention = camF*input.y + camR*input.x;
             MarioRotation();
            velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);
        }
        else if (MovementMode == Movement.Natural)
        {
            intention = transform.forward * input.y + transform.right * input.x;

            if (input.y * previousInputY <= 0 && input.y < 0)
            {
                intention += transform.forward * -5;
                 MarioRotation();
                velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);
            }

            else
            {
                intention += transform.forward * 5;
                 MarioRotation();
                velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);
            }
        }
        else if (MovementMode == Movement.Inverse)
        {
            intention = transform.forward * -input.y + transform.right * -input.x;


            if (input.y * previousInputY <= 0 && input.y > 0)
            {
                intention += transform.forward * -5;
                MarioRotationAlternate();
                velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);

            }

            else
            {
                intention += transform.forward * 5;
                MarioRotationAlternate();
                velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);

            }
        }

        float topSpeed = velocity.magnitude/turnSpeed;
        
        //As Velocity increases, our turn speed should be slower
        //within the range of 0 movement speed to topSpeed
        turnSpeed = Mathf.Lerp(turnSpeedHigh,turnSpeedLow, topSpeed );
        //If there is input...
      
        
        //then, we create a velocity that goes forward, which changes depending on the rotation
        //First, though, we get rid of the Velocity that affects the Y axis
        //Allowing for gravity to be used
        velocityXZ = velocity;
        velocityXZ.y = 0;
        velocityXZ = Vector3.Lerp(velocityXZ, transform.forward*input.magnitude * speed, accel * Time.deltaTime);
        //Now that we made sure everything but the Y is being affected, we finally change the velocity
        //We just use the default velocity.Y as that is being affected by gravity alone
       
       
          
       

        previousInputY = input.y;

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
            }

           

        }

        if (grounded == true)
        {
            JumpCount = 1;
        }

    }

    //Jat stuff
    public void MarioRotation() {

        //....We will get the rotation of the camera, determing the direction we face
        Quaternion rot = Quaternion.LookRotation(intention);
        ////And rotate the player in that direction.
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);

    }

    //Jay Stuff that fixed mario spinning when rapidly pressing W
    public void MarioRotationAlternate() {



        if (Input.GetKey(KeyCode.A)) {

            this.transform.Rotate(0, -3, 0);
         
           }


        if (Input.GetKey(KeyCode.D))
        {

            this.transform.Rotate(0, 3, 0);

        }

    }
}


