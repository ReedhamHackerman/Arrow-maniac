using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerUnit : MonoBehaviour
{
    [SerializeField] private int playerId;
    [SerializeField] private float speedHorizontal;
    [SerializeField] private float groundDetectionRange;
    [SerializeField] private float jumpForce;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _myCharacterSprite;

    public Transform[] allPositions;

    private Player player;
    private InputManager inputManager;
    private LayerMask groundLayerMask;
    public bool canJump;

    public bool Grounded { get; set; } = true;

    public void Initialize(int playerId)
    {
        this.playerId = playerId;
        InitializePlayersById();
        
        _rb = GetComponent<Rigidbody2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    public void UpdateUnit()
    {
        Grounded = isGrounded();
        Jump();
    }

    public void FixedUpdateUnit()
    {
        Move();
    }

    private void Move()
    {
        _rb.velocity = new Vector2(inputManager.HorizontalInput * speedHorizontal, _rb.velocity.y);
    }

    private void Jump()
    {
        if (inputManager.GetJumpButtonDown && Grounded)
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
}
