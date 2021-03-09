using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MoveDemo : MonoBehaviour
{
    [SerializeField] private int playerId;
    [SerializeField] private float speed;

    private Player player;
    private Rigidbody2D _rb;
    private LayerMask groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        player = ReInput.players.GetPlayer(0);
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetButtonDown("Jump"))
        {
            Debug.Log("jumping...");
        }

        isGrounded();
    }

    private void FixedUpdate()
    {
        float horizontal = player.GetAxis("Move Horizontal");
        float gravityCalculate = Physics2D.gravity.y;

        Vector2 moveVector = new Vector2(horizontal, 0);

        _rb.velocity = moveVector * speed * Time.fixedDeltaTime;
    }

    private void isGrounded()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, -Vector2.up, groundLayerMask);

        if (hitInfo)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.yellow);
        }
    }
}
