using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    [SerializeField] private float speedHorizontal;
    [SerializeField] private float groundDetectionRange;
    [SerializeField] private float jumpForce;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _myCharacterSprite;

    private LayerMask groundLayerMask;

    public bool Grounded { get; set; } = true;

    public virtual void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    protected virtual void UpdateUnit()
    {

    }

    private Vector2 Move()
    {
        return Vector2.zero;
    }

    private bool isGrounded()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, -Vector2.up, groundDetectionRange, groundLayerMask);
        return hitInfo ? true : false;
    }
}
