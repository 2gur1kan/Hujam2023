using System.Collections;
using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    [Header("GroundCheckLenght")]
    [SerializeField] private float ground = 1;

    [Header("Movment")]
    [SerializeField] private float moveSpeed = 5f; // Hareket hýzý

    private bool TurnRight = true;
    private bool TurnLeft = false;

    private bool isGrounded = false;
    private int GroundMask;
    private Rigidbody2D rb2d;

    public float height;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f; // Zýplama kuvveti
    private bool jump = true;
    private bool jumped = false;
    private bool jumpRestart = false;

    [Header("Dash")]
    [SerializeField] private float DashForce = 7;
    [SerializeField] private float DashCast = .5f;
    private bool dashed = false;
    private bool dashWaited = true;

    //stun value
    public bool DontMove;
    public bool Stun;
    public bool Dead;

    //animator
    private Animator anim;

    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
    public bool JumpBool { get => jump; set => jump = value; }
    public float DashForce1 { get => DashForce; set => DashForce = value; }

    private void Start()
    {
        GroundMask = LayerMask.GetMask("Ground");

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        if(transform.localScale.x > 0) transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        ChangeDirection();

        height = transform.position.y;
    }

    private void Update()
    {
        if (!Stuned())
        {
            CreateRayCast();
            Jump();
            UseDash();
            Move();
        }
    }

    private void FixedUpdate()
    {
        if (!Stuned())
        {
            CreateRayCast();
            Jump();
            UseDash();
            Move();
        }
    }

    public bool Stuned()
    {
        if (Stun) return true;
        if (DontMove) return true;

        return false;
    }

    private void CreateRayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, ground, GroundMask);
        Debug.DrawRay(transform.position, Vector2.down * ground, Color.red);
        isGrounded = hit.collider != null && hit.collider.CompareTag("Ground");

        if (isGrounded != anim.GetBool("IsGrounded")) anim.SetBool("IsGrounded", isGrounded);

        if (IsGrounded)
        {
            if(dashWaited) dashed = false;
        }

        if (Dead)
        {
            rb2d.velocity = Vector2.zero;
            if (isGrounded) rb2d.gravityScale = 0f;
        }
    }

    private void Move()
    {
        if (isGrounded) rb2d.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb2d.velocity.y);
        else rb2d.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, rb2d.velocity.y);

        WalkAnimationPlay();
        Turn();

        if (isGrounded)
        {
            height = transform.position.y;
        }
    }

    private void Jump()
    {
        if (jump)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumped = true;
                jump = false;

                anim.SetTrigger("Up");
            }

        }
        else if (jumpRestart)
        {
            if (isGrounded)
            {
                jump = true;
                jumpRestart = false;
            }
        }
        else if (jumped)
        {
            if (!isGrounded)
            {
                jumpRestart = true;
                jumped = false;
            }
        }

    }

    private void Turn()
    {
        if (Input.GetAxis("Horizontal") > 0 && !TurnRight)
        {
            ChangeDirection();
            TurnRight = true;
            TurnLeft = false;
        }
        else if (Input.GetAxis("Horizontal") < 0 && !TurnLeft)
        {
            ChangeDirection();
            TurnLeft = true;
            TurnRight = false;
        }
    }

    private void ChangeDirection()
    {
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    private void UseDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashed)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        dashWaited = false;
        dashed = true;
        anim.SetTrigger("Dash");
        rb2d.velocity = Vector2.zero;

        Vector3 dashDirection;
        float startTime = Time.time;

        if (transform.localScale.x > 0)
        {
            dashDirection = transform.right * DashForce;
        }
        else
        {
            dashDirection = -transform.right * DashForce;
        }

        rb2d.gravityScale = 0f;

        while (Time.time < startTime + .3f)
        {
            transform.Translate(dashDirection * Time.deltaTime / 0.3f, Space.World);
            yield return null;
        }

        rb2d.gravityScale = 2f;

        yield return new WaitForSeconds(DashCast);

        dashWaited = true;

        anim.ResetTrigger("Dash");
    }

    public void WalkAnimationPlay()
    {
        if(!dashed) anim.ResetTrigger("Dash");
        anim.ResetTrigger("Up");

        if(rb2d.velocity.x == 0)
        {
            anim.ResetTrigger("Walk");
            anim.SetTrigger("Stop");
        }
        else
        {
            anim.ResetTrigger("Stop");
            anim.SetTrigger("Walk");
        }
    }
}
