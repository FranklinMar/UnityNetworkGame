using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using SWNetwork;
using UnityEngine.SceneManagement;

public class Fox1 : MonoBehaviour
{
    [SerializeField] private Collider2D Stand;
    [SerializeField] private Collider2D Crouch;
    //[SerializeField] private Collider2D Crouch;
    [SerializeField] private float speed = 5f;
    [SerializeField] private int lives = 3;
    [SerializeField] private float jump = 7.5f;
    /*[SerializeField] private float crouchOffsetX = 0;
    [SerializeField] private float crouchOffsetY = 0.077f;
    [SerializeField] private float crouchSizeX = 0.175f;
    [SerializeField] private float crouchSizeY = 0.15f;*/
    
    /*private float standOffsetX;
    private float standOffsetY;
    private float standSizeX;
    private float standSizeY;*/
    
    private Rigidbody2D rigidBody;
    private SpriteRenderer sprite;
    private Animator animator;
    //private NetworkID networkID;
    // private Collider2D collider;
    private float moveInput;
    private float yVelocity
    {
        get { return animator.GetFloat("yVelocity"); }
        set { bool isGrounded = CheckGround();
            if (!CheckGround())
            {
                animator.SetFloat("yVelocity", value);
            } 
            else
            {
                animator.SetFloat("yVelocity", 0);
            }
            animator.SetBool("Jump", !isGrounded);
            //animator.SetFloat("yVelocity", value);
        }
    }
    private float xVelocity
    {
        get { return animator.GetFloat("xVelocity"); }
        set { animator.SetFloat("xVelocity", System.Math.Abs(value)); }
    }

    [SerializeField] private float checkRadius = 0.3f;
    [SerializeField] private Transform feetPos;
    [SerializeField] private LayerMask groundType;
    [SerializeField] private float jumpTime = 0.20f;
    [SerializeField] private float runSpeedIncreasing = 1.5f;
    [SerializeField] private float runCrouchDecreasing = 0.5f;
    private float jumpTimeCounter;
    private bool isJumping;
    private bool isRunning;
    private bool isCrouching;

    private bool crouching
    {
        get
        {
            return isCrouching;
        }
        set
        {
            isCrouching = value;
            animator.SetBool("Crouch", isCrouching);
            Stand.enabled = !value;
            Crouch.enabled = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        //networkID = GetComponent<NetworkID>();
        //collider = GetComponent<Collider2D>();
        /*standOffsetX = collider.offset.x;
        standOffsetY = collider.offset.x;
        standSizeX = collider.size.x;
        standSizeY = collider.size.y;*/

        //if (networkID.IsMine)
        //{
            //CameraController camera = Camera.main.GetComponent<CameraController>();
            //camera.player = transform;
        //}
    }

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        //networkID = GetComponent<NetworkID>();
        //collider = GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (networkID.IsMine) { 
            yVelocity = rigidBody.velocity.y;
            xVelocity = rigidBody.velocity.x;
            moveInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isRunning = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isRunning = false;
            }

            if (Input.GetButton("Crouch") && CheckGround())
            {
                crouching = true;
                /*collider.offset.x = crouchOffsetX;
                collider.offset.y = crouchOffsetY;
                collider.size.x = crouchSizeX;
                collider.size.y = crouchSizeY;*/
                /*isCrouching = true;
                animator.SetBool("Crouch", isCrouching);
                Crouch.enabled = true;
                Stand.enabled = false;*/
            }
            else if ((!Input.GetButton("Crouch") && !IsUnder(Crouch)) || !CheckGround())
            {
                crouching = false;
                /*isCrouching = false;
                animator.SetBool("Crouch", isCrouching);
                Stand.enabled = true;
                Crouch.enabled = false;*/
            }

            if (Input.GetButton("Horizontal"))
            {
                Run();
            }

            if (CheckGround() && Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                //animator.SetBool("Jump", true);
                jumpTimeCounter = jumpTime;
                Jump();
            }

            if (Input.GetButton("Jump") && isJumping)
            {
                if (jumpTimeCounter >= 0.0f)
                {
                    jumpTimeCounter -= Time.deltaTime;
                    Jump();
                }
                else
                {
                    isJumping = false;
                    //animator.SetBool("Jump", false);
                }
            }

            if (Input.GetButtonUp("Jump"))
            {
                isJumping = false;
            }

            if (Input.GetButton("Cancel"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        //}
    }

    private void Run()
    {
        float currentSpeed = moveInput * speed;
        if (isRunning && !isCrouching)
            currentSpeed *= runSpeedIncreasing;
        if (isCrouching && CheckGround()) //!isJumping
            currentSpeed *= runCrouchDecreasing;
        rigidBody.velocity = new Vector2(currentSpeed, rigidBody.velocity.y);
        sprite.flipX = moveInput < 0.0f;
    }

    private void Jump()
    {
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y <= jump ? jump : rigidBody.velocity.y);
        yVelocity = rigidBody.velocity.y;
    }

    private bool CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadius);
        bool isGrounded = colliders.Length > 1;
        //animator.SetBool("Jump", !isGrounded);
        return isGrounded;
    }

    private bool IsUnder(Collider2D current)
    {
        /*Collider2D current = isCrouching ? Crouch : Stand;
        Vector3 vector = new Vector3(current.transform.position.x, current.transform.position.y + current.bounds.extents.y);
        return Physics2D.OverlapCircle(vector, checkRadius);*/
        //Collider2D current = isCrouching ? Crouch : Stand;
        Collider2D[] otherColliders = Physics2D.OverlapCircleAll(current.transform.position, checkRadius);
        // Check for any colliders that are on top
        //bool isUnderneath = false;
        foreach (var collider in otherColliders)
        {
            if (collider.transform.position.y > current.transform.position.y)
            {
                return true;
            }
        }
        return false;
        /*RaycastHit2D JumpCheck = Physics2D.Raycast(transform.position, Vector2.up, 2f);
        return JumpCheck.collider == null;*/

    }
}
