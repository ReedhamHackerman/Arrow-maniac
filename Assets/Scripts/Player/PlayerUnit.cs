using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerUnit : MonoBehaviour,IFreezable
{
    [Header("LOCAL MULTIPLAYER")]
    [SerializeField] private int playerId;

    public int PlayerId => playerId;

    [Header("MOVEMENT")]
    [SerializeField] private float movementSpeed;

    [Header("JUMP")]
    [SerializeField] private Vector2 downDetectPosition;
    [SerializeField] private Vector2 downDetectScale;
    [SerializeField] private float jumpForce;

    [Header("DASH")]
    [SerializeField] private float maxDashTime;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;

    [Header("WALL SLIDING")]
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private Vector2 leftDetectPosition;
    [SerializeField] private Vector2 leftDetectScale;
    [SerializeField] private Vector2 rightDetectPosition;
    [SerializeField] private Vector2 rightDetectScale;

    [Header("WALL JUMP")]
    [SerializeField] private float wallJumpForce;
    [SerializeField] private Vector2 wallJumpAngle;

    [Header("AIM-SHOOT")]
    [SerializeField] private Transform handTransform;
    [SerializeField] private ArrowType currentEquippedArrow;
    [SerializeField] private Transform fireFromPos;

    [Header("OTHER SETTINGS")]
    [SerializeField] private bool showGizmos;

    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _myCharacterSprite;

    private float dashTimeCalculate;
    private float dashCooldownTimerCalculate;
    private float angleAim;

    private bool isDashing;
    private bool isMoving;
    private bool isWallSliding;
    private bool isAiming;
    private bool canUseDash;
    private bool canJump;
    private bool canRotate = true;

    private Vector2 storedPlayerVelocity;

    private LayerMask groundLayerMask;

    private Player player;
    private InputManager inputManager;
    private Invisible invisibleScript;
    private TimeStop timeStopScript;
    public TimeManager timeManager;

    private Transform[] allPositions; //Temporary array to store positions

    private Dictionary<int, Vector2> playersVelocity = new Dictionary<int, Vector2>();

    public bool Grounded { get; set; } = true;
    public bool LeftHit { get; set; } = true;
    public bool RightHit { get; set; } = true;
    public void Initialize(int playerId)
    {
        this.playerId = playerId;
        InitializePlayersById();

        _rb = gameObject.GetComponent<Rigidbody2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
        invisibleScript = GetComponent<Invisible>();
        timeStopScript = GetComponent<TimeStop>();
        invisibleScript.inputManager = this.inputManager;
        timeStopScript.inputManager = this.inputManager;

        dashTimeCalculate = maxDashTime;
        dashCooldownTimerCalculate = dashCooldown;

        isMoving = true;
        canUseDash = true;
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
        Aim();
    }

    public void FixedUpdateUnit()
    {
        Move();
    }


    #region MOVEMENT MECHANICS
    private void Move()
    {
        if(!isAiming)
        {
            if (isMoving)
            {
                _rb.velocity = new Vector2(inputManager.HorizontalInput * movementSpeed, _rb.velocity.y);
            }

            if (isDashing)
            {
                _rb.velocity = new Vector2(inputManager.HorizontalInput * dashSpeed, _rb.velocity.y);
            }
        }
    }

    private void Rotate()
    {
        if (!canRotate) return;
        if (inputManager.HorizontalInput != 0)
        {
            transform.rotation = inputManager.HorizontalInput < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
        }
    }

    private void Jump()
    {
        if (inputManager.GetJumpButtonDown && (Grounded || LeftHit || RightHit))
        {
            canJump = true;
        }

        if (canJump)
        {
            if(isWallSliding)
            {
                if(LeftHit)
                    _rb.AddForce(new Vector2(wallJumpForce * wallJumpAngle.x, wallJumpForce * wallJumpAngle.y), ForceMode2D.Impulse);
                else if(RightHit)
                    _rb.AddForce(new Vector2(wallJumpForce * -wallJumpAngle.x, wallJumpForce * wallJumpAngle.y), ForceMode2D.Impulse);
            }
            else
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);

            canJump = false;
        }
    }

    private void Dash()
    {
        if(!canUseDash)
        {
            dashCooldownTimerCalculate -= Time.deltaTime;

            if (dashCooldownTimerCalculate <= 0)
            {
                dashCooldownTimerCalculate = dashCooldown;
                canUseDash = true;
            }
        }

        if (inputManager.GetDashButtonDown && canUseDash && !isDashing && !isWallSliding)
        {
            isDashing = true;
            isMoving = false;
            canUseDash = false;
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
        if ((LeftHit || RightHit) && !Grounded && _rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            isMoving = false;
            _rb.velocity = new Vector2(_rb.velocity.x, -wallSlideSpeed);
        }
        else
        {
            if(Grounded && _rb.velocity.y == 0 && !isMoving)
                isMoving = true;
        }
    }

    IEnumerator EnableMovement()
    {
        yield return new WaitForSeconds(0.2f); //DON'T change the time
        isMoving = true;
    }

    #endregion

    private void Aim()
    {
        if (inputManager.GetAimButton)
        {
            isAiming = true;

            if (_rb.velocity.x != 0) _rb.velocity = Vector2.zero;
        }

        if (inputManager.GetAimButtonUp)
        {
            isAiming = false;
            Shoot();
        }

        if (isAiming)
        {
            Vector2 aim = new Vector2(inputManager.HorizontalInput, inputManager.VerticalInput);

            angleAim = Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg;
            if (angleAim < 0f) angleAim += 360f;

            if (aim.x < 0)
            {
                handTransform.localEulerAngles = new Vector3(180, 180, -angleAim);
            }
            else
            {
                handTransform.localEulerAngles = new Vector3(0f, 0f, angleAim);
            }
        }
        else
            handTransform.localEulerAngles = Vector2.zero;
    }

    private void Shoot()
    {
        Arrow newArrow = ArrowManager.Instance.Fire(currentEquippedArrow, fireFromPos.position, fireFromPos.rotation);
        newArrow.Oninitialize();
        newArrow.AddForceInDirection(fireFromPos.right);
    }

    #region ON COLLISION CODE
    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((groundLayerMask | 1 << collision.gameObject.layer) == groundLayerMask)
            StartCoroutine(EnableMovement());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((groundLayerMask | 1 << collision.gameObject.layer) == groundLayerMask)
            isMoving = true;
    }
    #endregion

    #region GROUND CHECKING
    private bool isGrounded()
    {
        bool isHit = Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y + downDetectPosition.y), downDetectScale, 0f, groundLayerMask);
        return isHit;
    }

    private bool isLeftHit()
    {
        bool isHit = Physics2D.OverlapBox(new Vector2(transform.position.x + leftDetectPosition.x, transform.position.y + leftDetectPosition.y), leftDetectScale, 0f, groundLayerMask);
        return (isHit);
    }

    private bool isRightHit()
    {
        bool isHit = Physics2D.OverlapBox(new Vector2(transform.position.x + rightDetectPosition.x, transform.position.y + rightDetectPosition.y), rightDetectScale, 0f, groundLayerMask);
        return (isHit);
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(new Vector2(transform.position.x, transform.position.y + downDetectPosition.y), downDetectScale);

            Gizmos.color = Color.blue;
            Gizmos.DrawCube(new Vector2(transform.position.x + leftDetectPosition.x, transform.position.y + leftDetectPosition.y), leftDetectScale);

            Gizmos.color = Color.blue;
            Gizmos.DrawCube(new Vector2(transform.position.x + rightDetectPosition.x, transform.position.y + rightDetectPosition.y), rightDetectScale);
        }
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

    public void Freeze()
    {
        if (this.playerId != PlayerManager.Instance.playerIdUsedAbility)
        {
            storedPlayerVelocity = _rb.velocity;
            _rb.bodyType = RigidbodyType2D.Static;
            canRotate = false;
        }       
    }

    public void UnFreeze()
    {
        if (playerId != PlayerManager.Instance.playerIdUsedAbility)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.velocity = storedPlayerVelocity ;            
            canRotate = true;
            
        }
    }

    public void Die()
    {
        print("player id " + playerId);
        Destroy(gameObject);
    }
}
