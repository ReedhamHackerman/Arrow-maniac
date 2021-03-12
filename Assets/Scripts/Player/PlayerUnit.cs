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

    private Player player;
    private InputManager inputManager;
    private LayerMask groundLayerMask;

    public bool Grounded { get; set; } = true;

    public virtual void Initialize()
    {
        player = ReInput.players.GetPlayer(playerId);
        inputManager = new InputManager(this.player);

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
        if (inputManager.GetJumpButtonDown && Grounded) Debug.Log("jump..");
            //_rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool isGrounded()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, -Vector2.up, groundDetectionRange, groundLayerMask);
        return hitInfo ? true : false;
    }
}
