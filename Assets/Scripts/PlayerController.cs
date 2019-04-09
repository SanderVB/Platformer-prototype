using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    //Customizable values for character controls
    [Header("Character values")]
    [Tooltip("in seconds")][SerializeField] float maxHealth = 30f;
    [Tooltip("seconds to substract")] [SerializeField] float contactDamage = 10f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float jumpSpeed = 15f;
    [SerializeField] float climbSpeed = 15f;
    [SerializeField] float slideSpeed = 5f;
    [SerializeField] float jumpCooldown = .25f;
    [SerializeField] float landThreshold = 2f;
    [SerializeField] float scaleSize = 1.5f;

    [Header("Hurt status")]
    [SerializeField] float hurtCooldown = 2f;
    [SerializeField] Vector2 hurtKick = new Vector2(10f, 10f);

    [Header("Sounds")]
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip deathSound;

    bool isAlive = true;
    bool onCooldown, levelFinished = false;
    bool isGrounded, isJumping, isSwimming, hasHorizontalSpeed, facingRight;

    float hurtTimer, jumpTimer, baseAnimSpeed;
    public float groundedTimer;
    public float currentHealth;
    float animationCooldown = 0.5f;
    Vector2 baseVelocity;

    //Cached references
    Rigidbody2D myRB2D;
    Animator myAnim;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;

    // Use this for initialization
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        currentHealth = maxHealth;
        myRB2D = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        baseAnimSpeed = myAnim.speed;
        baseVelocity = myRB2D.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive || levelFinished) { return; } //if the player isn't alive, no functions are called and the control by input is taken away

        currentHealth -= Time.deltaTime;

        if (!onCooldown)
        {
            Run();
            Jump();
            Swimming();
            FlipPlayer();
        }

        GroundCheck();
        HurtHandler();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); //value is between -1 to 1

        if (controlThrow >= 0) //sets if character is facing right or left, used for the hurt-kickback
            facingRight = true;
        else
            facingRight = false;
        Vector2 playerVelocity;
        if (!isSwimming)
            playerVelocity = new Vector2(controlThrow * runSpeed, myRB2D.velocity.y); //sets character velocity based on the received input value
        else
            playerVelocity = new Vector2(controlThrow * runSpeed/2, myRB2D.velocity.y);
        myRB2D.velocity = playerVelocity;

        myAnim.SetBool("isRunning", hasHorizontalSpeed);
    }

    private void GroundCheck()
    {
        isGrounded = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")); //checks is the collider on the bottom of the character is contact with objects tagged as Ground
        //myAnim.SetBool("isGrounded", isGrounded);
        if(!isGrounded)
        {
            if (groundedTimer < .2f) //I wouldn't normally hardcode, but this is a one-off case that is never used again after fine-tuning (it prevents the hangtime animation from triggering too early)
            {
                groundedTimer += Time.deltaTime;
            }
            else
            {
                groundedTimer = 0f;
                myAnim.SetBool("isGrounded", false);
            }
        }
        else
        {
            groundedTimer = 0;
            myAnim.SetBool("isGrounded", true);
        }

        if (!isGrounded && myRB2D.velocity.y < -landThreshold) //used to trigger the landing animation, based on how high the velocity is (i.e. from how high the chararcter dropped)
        {
            myAnim.SetBool("needsLanding", true);
        }
        else if (!isGrounded)
        {
            myAnim.SetBool("needsLanding", false);
        }
    }

    private void Jump()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump") && isGrounded && !isSwimming) //is the jump button pressed and is the character on solid ground? Then jump
        {
            isJumping = true;
            AudioSource.PlayClipAtPoint(jumpSound, this.transform.position);
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRB2D.velocity += jumpVelocityToAdd;
        }

        if(isJumping) //this was added to fix the animation not switching properly from idle to jumping when pressing jump repeadetly
        {
            if (jumpTimer < jumpCooldown)
                jumpTimer += Time.deltaTime;
            else if(isGrounded)
            {
                isJumping = false;
                jumpTimer = 0;
            }
        }

        myAnim.SetBool("isJumping", isJumping);
    }

    private void Swimming()
    {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Water")))
        {
            isSwimming = true; 
            isJumping = true;
            myAnim.speed = baseAnimSpeed / 2;
            Vector2 climbVelocity = new Vector2(myRB2D.velocity.x, climbSpeed);
            Vector2 slideVelocity = new Vector2(myRB2D.velocity.x, -slideSpeed);
            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                myRB2D.velocity = climbVelocity;
                isJumping = true;
            }
            else
            {
                isJumping = false;
                if (myRB2D.velocity.y > -slideSpeed) //makes the character slide down gradually 
                {
                    float currentSpeed = myRB2D.velocity.y;
                    myRB2D.velocity = new Vector2(myRB2D.velocity.x, currentSpeed - (slideSpeed / 100));
                }
                else if (myRB2D.velocity.y < -slideSpeed)
                {
                    myRB2D.velocity = slideVelocity;
                }
            }
        }
        else
        {
            myAnim.speed = baseAnimSpeed;
            isSwimming = false;
        }
    }

    private void FlipPlayer()
    {
        hasHorizontalSpeed = Mathf.Abs(myRB2D.velocity.x) > Mathf.Epsilon; //needs rewrite, was written when update was functioning differently NOTE: use facing-right bool
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRB2D.velocity.x) * scaleSize, scaleSize);
        }
    }

    private void HurtHandler()
    {
        if (hurtTimer < hurtCooldown)       //character is not being able to be hurt after spawning or after just being hit
            hurtTimer += Time.deltaTime;

        else if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) || bodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            if (!onCooldown) //subtracts health value & knocks the character back in the opposite direction NOTE: needs finetuning
            {
                isGrounded = false;
                AudioSource.PlayClipAtPoint(hurtSound, this.transform.position);
                currentHealth -= contactDamage;
                if (facingRight)
                {
                    myRB2D.velocity = new Vector2(hurtKick.x * -1, hurtKick.y);
                }
                else
                {
                    myRB2D.velocity = hurtKick;
                }
                if (isGrounded)
                {
                    myRB2D.velocity = new Vector2(0f, 0f);
                }
                StartCoroutine(CooldownTimer()); //see below for description
                hurtTimer = 0;
            }
        }

        if (currentHealth <= 0 || bodyCollider.IsTouchingLayers(LayerMask.GetMask("Killzone")))
        {
            DeathHandler();
        }
    }

    IEnumerator CooldownTimer() //makes the hurt animation stay active for a set time
    {
        if (!onCooldown)
        {
            onCooldown = true;
            myAnim.SetBool("cooldown", onCooldown);
            yield return new WaitForSeconds(animationCooldown);
            onCooldown = false;
            myAnim.SetBool("cooldown", onCooldown);
        }

    }
    private void DeathHandler()
    {
        isAlive = false;
        AudioSource.PlayClipAtPoint(deathSound, this.transform.position);
        FindObjectOfType<GameSession>().PlayerDied();
        myRB2D.bodyType = RigidbodyType2D.Kinematic; 
        myRB2D.velocity = new Vector2(0f, 0f); //ensures the player model stays in the same space after dying, preventing camera drifting
        myAnim.SetBool("isAlive", isAlive);
    }

    public float GetPlayerHealth()
    {
        return currentHealth;
    }

    public void ReplenishHealth(float healthToAdd)
    {
        currentHealth += healthToAdd;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void SetLevelFinished()
    {
        myAnim.SetBool("celebration", true); //TODO add celebration animation
        myRB2D.velocity = new Vector2(0f, 0f); //ensures the player model stays in the same space after dying, preventing camera drifting

        levelFinished = true;
    }
}
