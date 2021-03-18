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
    [SerializeField] private float groundDetectionRange;
    [SerializeField] private float jumpForce;

    [Header("Dash")]
    [SerializeField] private float maxDashTime;
    [SerializeField] private float dashSpeed;

    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _myCharacterSprite;

    public Vector2 facingDirection;

    private float dashTimeCalculate;
    public bool isDashing;

    private Transform[] allPositions;

    private Player player;
    private InputManager inputManager;
    private Invisible invisibleScript;
    public TimeManager timeManager;
    private LayerMask groundLayerMask;

    public bool Grounded { get; set; } = true;

    public void Initialize(int playerId)
    {
        this.playerId = playerId;
        InitializePlayersById();
        
        _rb = GetComponent<Rigidbody2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
        invisibleScript = GetComponent<Invisible>();
        invisibleScript.inputManager = this.inputManager;

        timeManager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();

        dashTimeCalculate = maxDashTime;
    }

    public void UpdateUnit()
    {
        Grounded = isGrounded();
        Jump();
        Rotate();
        Dash();
        UseAbility();
    }

    public void FixedUpdateUnit()
    {
        Move();
    }

    private void Move()
    {
        if(!isDashing)
            _rb.velocity = new Vector2(inputManager.HorizontalInput * speedHorizontal, _rb.velocity.y);
    }

    private void Rotate()
    {
        if(inputManager.HorizontalInput != 0)
        {
            if(inputManager.HorizontalInput < 0)
                transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (inputManager.HorizontalInput > 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void Jump()
    {
        if (inputManager.GetJumpButtonDown && Grounded)
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void Dash()
    {
        if (inputManager.GetDashButtonDown && !isDashing) isDashing = true;

        if(isDashing)
        {
            dashTimeCalculate -= Time.deltaTime;

            _rb.velocity = new Vector2(inputManager.HorizontalInput * dashSpeed, _rb.velocity.y);

            if (dashTimeCalculate < 0)
            {
                isDashing = false;
                dashTimeCalculate = maxDashTime;
            }
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, -Vector2.up, groundDetectionRange, groundLayerMask);
        return hitInfo ? true : false;
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

    private void UseAbility()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            timeManager.TimeIsStopped = true;
            Debug.Log("Called it");

        }

        if (inputManager.UseAbility)
        {

        }

        if (Input.GetKeyDown(KeyCode.E) && timeManager.TimeIsStopped)
        {
            timeManager.ContinueTime();
        }
    }
}
