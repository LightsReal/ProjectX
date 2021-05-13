using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //Assingables
    public Transform playerCam;
    
    //Other
    private Rigidbody2D rb;
    
    //Movement
    public float moveSpeed;
    public float maxSpeed;
    public float groundCheckRaycastDistance;
    public bool grounded;
    public LayerMask whatIsGround;

    //Crouch
    private Vector2 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector2 playerScale;

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.05f;
    public float jumpForce;
    
    //Dashing
    public float dashForce;
    private float dashCooldown = 3f;
    private bool readyToDash = true;

    //Input
    private float x, y;
    private bool moving, jumping, crouching, dashing;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Start() {
        playerScale =  transform.localScale;
    }
    
    private void FixedUpdate() {
        Movement();
        MyInput();
    }

    /// <summary>
    /// Find user input
    /// </summary>
    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        moving = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        jumping = Input.GetKeyDown(KeyCode.S);
        crouching = Input.GetKey(KeyCode.LeftControl);
        dashing = Input.GetKey(KeyCode.LeftShift);
    }

    private void Movement()
    {
        //Other
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        //Extra gravity
        rb.AddForce(Vector2.down * Time.deltaTime * 10);
        
        //Staying && no keys pressing
        if (!moving && !jumping && !crouching && !dashing)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        //Moving
        if (moving && !crouching) Move(); 

        //If holding jump && ready to jump, then jump
        if (jumping && readyToJump && !crouching) Jump();

        //Dashing
        if (dashing && readyToDash && !crouching) Dash();

        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();

        //Set max speed
        float maxSpeed = this.maxSpeed;
        
        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && x > maxSpeed) x = 0;    
        if (x < 0 && x < -maxSpeed) x = 0;    
        if (y > 0 && y < maxSpeed) y = 0;    
        if (y < 0 && y < -maxSpeed) y = 0;    
    }
    
    private void Move()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        //Add move forces
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-moveSpeed * 1.5f, rb.velocity.y);        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(moveSpeed * 1.5f, rb.velocity.y);
            }
        }
    }

    private void Jump()
    {
        if (grounded && readyToJump) {
            readyToJump = false;
            grounded = false;
            
            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            //rb.AddForce(normalVector * jumpForce * 0.5f); WALL JUMP!
            
            //If jumping while falling, reset y velocity.
            Vector2 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector2(vel.x, 0);
            else if (rb.velocity.y > 0) 
                rb.velocity = new Vector2(vel.x, vel.y / 2);
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    
    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Dash()
    {
        if (readyToDash)
        {
            readyToDash = false;
            
            //Add dash forces
            rb.velocity = new Vector2(rb.velocity.x * dashForce * 1.5f, rb.velocity.y);

            Invoke(nameof(ResetDash), dashCooldown);
        }
    }

    private void ResetDash()
    {
        readyToDash = true;
    }

    private void StartCrouch()
    {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
    }

    private void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private bool cancellingGrounded;

    /// <summary>
    /// Handle ground detection
    /// </summary>
    private void OnCollisionEnter2D(Collision2D other)
    {
    grounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckRaycastDistance, whatIsGround) ||
               Physics2D.Raycast(new Vector2(transform.position.x - 0.5f, transform.position.y), Vector2.down, groundCheckRaycastDistance, whatIsGround) ||
               Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y), Vector2.down, groundCheckRaycastDistance, whatIsGround);
    }
   
}