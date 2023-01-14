using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int lives = 3;
    /*[SerializeField]*/ private float jump = 10f;
    private float moveInput;

    private Rigidbody2D rigidBody;
    private SpriteRenderer sprite;
    private Animator animator;

    [SerializeField] private float checkRadius = 0.3f;
    [SerializeField] private Transform feetPos;
    [SerializeField] private LayerMask groundType;
    /*[SerializeField]*/ private float jumpTime = 0.20f;
    private float jumpTimeCounter;
    private bool isJumping;
    private State state
    {
        get { return (State)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();//transform.Find("Hero").GetComponent<SpriteRenderer>();
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();//transform.Find("Hero").GetComponent<SpriteRenderer>();
    }

    private void Run()
    {
        /*Vector3 direction = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0f;*/
        if (CheckGround())
        {
            state = State.run;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        //moveInput = -3;

        rigidBody.velocity = new Vector2(moveInput * speed, rigidBody.velocity.y);
        //rigidBody.velocity.x = moveInput * speed;
        // rigidBody = rigidBody;
        sprite.flipX = moveInput < 0.0f;
        //sprite.flipX = rigidBody.velocity.x < 0.0f;
        //sprite.flipX = rigidBody.velocity.normalized.x < 0.0f;
        //sprite.flipX = (transform.right * Input.GetAxis("Horizontal")).x < 0.0f;
        /*if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        } else if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }*/
    }

    /*private void Jump() {
        // rigidBody.AddForce(transform.up * jump, ForceMode2D.Impulse);
        rigidBody.velocity = Vector2.up * jump;
        jumpTimeCounter = jumpTime;
        if (Input.GetButton("Jump") && isJumping) {
            if (jumpTimeCounter > 0.0f) {
                rigidBody.velocity = Vector2.up * jump;
                jumpTimeCounter -= Time.deltaTime;
            } else {
                isJumping = false;
            }
        }
    }*/
    private void Jump()
    {
        // rigidBody.velocity = Vector2.up * jump;
        // rigidBody.AddForce(transform.up * jump, ForceMode2D.Impulse);
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y <= jump ? jump : rigidBody.velocity.y);
    }

    private bool CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadius); //0.3f
        bool isGrounded = colliders.Length > 1;
        if (!isGrounded)
        {
            if (rigidBody.velocity.y > 0)
            {
                state = State.jump;
            } else if (rigidBody.velocity.y < 0)
            {
                state = State.fall;
            }

        }
        return isGrounded;
        //return colliders.Length > 1;
        // return Physics2D.OverlapCircle(feetPos.position, checkRadius, groundType);
    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (CheckGround())
        {
            state = State.idle;
        }

        if (/*true || */Input.GetButton("Horizontal"))
        {
            Run();
        }
        /*if (CheckGround() && Input.GetButtonDown("Jump")) {
            isJumping = true;
            Jump();
        }*/
        if (CheckGround() && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
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
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }
}

public enum State
{
    idle,
    run,
    jump,
    fall
}