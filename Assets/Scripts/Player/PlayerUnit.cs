using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerUnit : MonoBehaviour
{
    [Header("Local Multiplayer")]
    [SerializeField] private int playerId;

    [Header("Movement")]
    [SerializeField] private float speedHorizontal;

    [Header("Jump")]
    [SerializeField] private Vector2 downDetectPosition;
    [SerializeField] private Vector2 downDetectScale;
    [SerializeField] private float jumpForce;

    [Header("Dash")]
    [SerializeField] private float maxDashTime;
    [SerializeField] private float dashSpeed;

    [Header("Wall Sliding")]
    [SerializeField] private Vector2 leftDetectPosition;
    [SerializeField] private Vector2 leftDetectScale;
    [SerializeField] private Vector2 rightDetectPosition;
    [SerializeField] private Vector2 rightDetectScale;

    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _myCharacterSprite;

    private float dashTimeCalculate;
    private bool isDashing;
    private bool isMoving;

    private Transform[] allPositions;

    private Player player;
    private InputManager inputManager;
    private LayerMask groundLayerMask;

    public bool Grounded { get; set; } = true;
    public bool LeftHit { get; set; } = true;
    public bool RightHit { get; set; } = true;

    public void Initialize(int playerId)
    {
        this.playerId = playerId;
        InitializePlayersById();

        _rb = GetComponent<Rigidbody2D>();
        groundLayerMask = LayerMask.GetMask("Ground");

        dashTimeCalculate = maxDashTime;
    }

    public void UpdateUnit()
    {
        Grounded = isGrounded();
        LeftHit = isLeftHit();
        RightHit = isRightHit();

        Jump();
        Rotate();
        Dash();
        WallSlide();
    }

    public void FixedUpdateUnit()
    {
        Move();
    }

    private void Move()
    {
        if (isMoving)
        {
            _rb.velocity = new Vector2(inputManager.HorizontalInput * speedHorizontal, _rb.velocity.y);
        }
        else if(isDashing)
        {
            _rb.velocity = new Vector2(inputManager.HorizontalInput * dashSpeed, _rb.velocity.y);
        }
    }

    private void Rotate()
    {
        if (inputManager.HorizontalInput != 0)
        {
            transform.rotation = inputManager.HorizontalInput < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        }
    }

    private void Jump()
    {
        if (inputManager.GetJumpButtonDown)
        {
            if (Grounded)
                _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            if (LeftHit)
            {
                _rb.AddForce(new Vector2(-inputManager.HorizontalInput, inputManager.VerticalInput) * jumpForce, ForceMode2D.Impulse);
                isMoving = false;
            }

            if (RightHit)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, 25f);
            }
        }
    }

    private void Dash()
    {
        if (inputManager.GetDashButtonDown && !isDashing)
        {
            isDashing = true;
            isMoving = false;
        }

        if (isDashing)
        {
            dashTimeCalculate -= Time.deltaTime;

            if (dashTimeCalculate < 0)
            {
                isDashing = false;
                isMoving = true;
                dashTimeCalculate = maxDashTime;
            }
        }
    }

    private void WallSlide()
    {
        if(!isDashing)
        {
            if (LeftHit || RightHit)
            {
                isMoving = false;

                _rb.velocity = new Vector2(_rb.velocity.x, -0.5f);
            }
            else
            {
                if (!isMoving) isMoving = true;
            }
        }
    }

    private bool isGrounded()
    {
        bool isHit = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y + downDetectPosition.y), downDetectScale, 0f, groundLayerMask);
        return isHit;
    }

    private bool isLeftHit()
    {
        bool isHit = Physics2D.OverlapBox(new Vector2(transform.position.x + leftDetectPosition.x, transform.position.y + leftDetectPosition.y), leftDetectScale, 0f, groundLayerMask);
        return (isHit && inputManager.HorizontalInput < 0 && !Grounded);
    }

    private bool isRightHit()
    {
        bool isHit = Physics2D.OverlapBox(new Vector2(transform.position.x + rightDetectPosition.x, transform.position.y + rightDetectPosition.y), rightDetectScale, 0f, groundLayerMask);
        return (isHit && inputManager.HorizontalInput > 0 && !Grounded);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y + downDetectPosition.y), downDetectScale);

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2(transform.position.x + leftDetectPosition.x, transform.position.y + leftDetectPosition.y), leftDetectScale);

        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2(transform.position.x + rightDetectPosition.x, transform.position.y + rightDetectPosition.y), rightDetectScale);
    }

    private void InitializePlayersById()
    {
        //Temporary spawn position code
        allPositions = new Transform[2];
        allPositions[0] = GameObject.Find("PosOne").transform;
        allPositions[1] = GameObject.Find("PosTwo").transform;

        player = ReInput.players.GetPlayer(playerId);
        inputManager = new InputManager(this.player);
        transform.position = allPositions[playerId].position;
    }
}
