using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

    [SerializeField] float runSpeed = 8f;
    [SerializeField] float jumpSpeed = 15f;
    [SerializeField] float scaleSize = 1.5f;
    [SerializeField] float climbSpeed = 15f;
    [SerializeField] float slideSpeed = 5f;

    bool isAlive = true;
    bool isGrounded, isJumping;
    bool hasHorizontalSpeed;

    //Cached references
    Rigidbody2D myRB2D;
    Animator myAnim;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;

	// Use this for initialization
	void Start () {
        myRB2D = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Run();
        GroundCheck();
        Jump();
        Climb();
        FlipPlayer();
	}

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); //value is between -1 to 1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRB2D.velocity.y);
        myRB2D.velocity = playerVelocity;

        myAnim.SetBool("isRunning", hasHorizontalSpeed);

    }

    private void GroundCheck()
    {
        isGrounded = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        myAnim.SetBool("isGrounded", isGrounded);

        if (isJumping) { }
    }

    private void Jump()
    {
        myAnim.SetBool("isJumping", isJumping);
        if (CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRB2D.velocity += jumpVelocityToAdd;
            myAnim.SetBool("isJumping", isJumping);
            isJumping = false;
        }
    }

    private void Climb()
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            Vector2 climbVelocity = new Vector2(myRB2D.velocity.x, climbSpeed);
            Vector2 slideVelocity = new Vector2(myRB2D.velocity.x, -slideSpeed);
            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                myRB2D.velocity = climbVelocity;
            }
            else
            {
                if(myRB2D.velocity.y > -slideSpeed)
                {
                    float currentSpeed = myRB2D.velocity.y;
                    myRB2D.velocity = new Vector2(myRB2D.velocity.x, currentSpeed - (slideSpeed/100));
                }
                else if(myRB2D.velocity.y < -slideSpeed)
                {
                    myRB2D.velocity = slideVelocity;
                }
            }
        }
    }


    private void FlipPlayer()
    {
        hasHorizontalSpeed = Mathf.Abs(myRB2D.velocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRB2D.velocity.x)* scaleSize, scaleSize);
        }
    }
}
