using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class JayWalkingSound : MonoBehaviour
{

    //OBJECT variables
    public Transform cam;
    private CharacterController mover;

    //CAMERA Variables
    private Vector3 camF;
    private Vector3 camR;

    //INPUT Variables
    private Vector2 input;


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


    //Audio
    public AudioSource walkingSound;


    //jump
    public float movementMultiplier;
    public float jumpMultiplier;
    public float fallMultiplier;
    public float walkingMultiplier;
    public float JumpCount;
    private float MaxJump = 1f;




    void Start()
    {
        mover = GetComponent<CharacterController>();
        turnSpeedLow = turnSpeed;
        turnSpeedHigh = turnSpeed * 4;

        JumpCount = 1f;

      
    }


    void Update()
    {
        DoInput();
        CalculateCamera();
        CalculateGround();
        DoMove();
        DoGravity();
        Jumping();

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {

            walkingSound.Play();


        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {

            walkingSound.Stop();
        }


        if (velocity.y < 0)
        {//player is falling down
            //add to the existing velocity and multiply by gravity and multiply by time since
            velocity = velocity + Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;

        }
        else if (velocity.y > 0)
        {//player is jumping up
            velocity = velocity + Vector3.up * Physics.gravity.y * jumpMultiplier * Time.fixedDeltaTime;
        }






        //We finally move once DoMove has calculated the velocity, rather than
        //at the same time
        mover.Move(velocity * Time.deltaTime);


    }

    public void DoInput()
    {

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxis("Vertical"));

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

        ////First we need to make sure if we are touching
        ////the ground first.
        ////This makes sure fall speed is consistent
        //if (grounded)
        //{
        //    velocity.y = -0.5f;
        //}
        //else
        //{
        //    //Just changing the velocity to be going downwards.
        //    velocity.y -= grav * Time.deltaTime;
        //}

        ////We also set a limit to how long velocity.y can be decreased/increased.
        //velocity.y = Mathf.Clamp(velocity.y, -10, 10);
    }

    public void Jumping()
    {
        if (grounded && JumpCount >=1f)
        {
            if (Input.GetKeyDown (KeyCode.Space))
            {
                velocity = Vector3.up * movementMultiplier;


                JumpCount = JumpCount - 1;
               
            }


        }
    }
}


