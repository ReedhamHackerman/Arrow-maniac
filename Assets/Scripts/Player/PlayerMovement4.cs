using UnityEngine;

public class PlayerMovement4 : MonoBehaviour
{
    [Header("For Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float airMoveSpeed = 10f;
    private float XDirectionalInput;
    private bool facingRight = true;
    private bool isMoving;

    [Header("For Jumping")]
    [SerializeField] float jumpForce = 16f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] Vector2 groundCheckSize;
    private bool grounded;
    private bool canJump;

    [Header("For WallSliding")]
    [SerializeField] float wallSlideSpeed;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] Vector2 wallCheckSize;
    private bool isTouchingWall;
    private bool isWallSliding;

    [Header("For WallJumping")]
    [SerializeField] float walljumpforce;
    [SerializeField] Vector2 walljumpAngle;
    [SerializeField] float walljumpDirection = -1;

    [Header("Other")]
    [SerializeField] Animator anim;
    private Rigidbody2D rb;
   


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        walljumpAngle.Normalize();
       
    }

    private void Update()
    {
        Inputs();
        CheckWorld();
       // AnimationControl();
       
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
        WallSlide();
        WallJump();
    }

    void Inputs()
    {
        XDirectionalInput = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            canJump = true;
        }
    }
    void CheckWorld()
    {
        grounded = Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
        isTouchingWall = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, wallLayer);
    }

    void Movement()
    {
        //for Animation
        if (XDirectionalInput !=0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //for movement
        if (grounded)
        {
            rb.velocity = new Vector2(XDirectionalInput * moveSpeed, rb.velocity.y);
        }
        else if (!grounded &&(!isWallSliding || !isTouchingWall) && XDirectionalInput!=0 )
        {
            rb.AddForce(new Vector2(airMoveSpeed*XDirectionalInput,0));
            if (Mathf.Abs(rb.velocity.x)>moveSpeed)
            {
                rb.velocity = new Vector2(XDirectionalInput * moveSpeed, rb.velocity.y);
            }
        }

        //for fliping
        if (XDirectionalInput<0 && facingRight)
        {
            Flip();
        }
        else if (XDirectionalInput > 0 && !facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        if (!isWallSliding)
        {
            walljumpDirection *= -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }

    }

    void Jump()
    {
        if (canJump && grounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            canJump = false;
        }

       
    }

    void WallSlide()
    {
        if (isTouchingWall && !grounded && rb.velocity.y<0 )
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
        }
    }
    void WallJump()
    {
        if ((isWallSliding) && canJump)
        {
            rb.AddForce(new Vector2(walljumpforce * walljumpAngle.x * walljumpDirection, walljumpforce * walljumpAngle.y),ForceMode2D.Impulse);
            Flip();
            canJump = false;
            
        }   
    }
    
    void AnimationControl()
    {
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", grounded);
        anim.SetBool("isSliding", isTouchingWall);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(groundCheckPoint.position, groundCheckSize);
        Gizmos.color = Color.green;
        Gizmos.DrawCube(wallCheckPoint.position, wallCheckSize);

    }
}
