using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MoveDemo : MonoBehaviour
{
    [SerializeField] private int playerId;
    [SerializeField] private float speed;
    [SerializeField] private float gravityValue;
    [SerializeField] private float groundDetectionRange;
    [SerializeField] private float jumpForce;

    public Vector2 moveVector;

    private Player player;
    private Rigidbody2D _rb;
    private LayerMask groundLayerMask;

    private float gravityCalculate;
    private float horizontal;
    public bool canJump;

    public bool Grounded { get; set; } = true;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);
        _rb = GetComponent<Rigidbody2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        Grounded = isGrounded();
        horizontal = player.GetAxis("Move Horizontal");

        Jumping();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        _rb.velocity = new Vector2(horizontal * speed, _rb.velocity.y);
    }

    private void Jumping()
    {
        if (player.GetButtonDown("Jump") && Grounded)
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool isGrounded()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, -Vector2.up, groundDetectionRange, groundLayerMask);

        return hitInfo ? true : false;
    }
}
