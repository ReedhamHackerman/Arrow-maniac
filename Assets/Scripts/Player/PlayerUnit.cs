using Rewired;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUnit : MonoBehaviour, IFreezable
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
    [SerializeField] private Transform fireFromPos;

    [Header("ARROW HUD")]
    [SerializeField] private GameObject arrowHudParent;
    private Dictionary<ArrowType, GameObject> AllArrowHUDSDictionary = new Dictionary<ArrowType, GameObject>();
    [SerializeField]private float arrowHudHeight;
    [SerializeField]private float arrowHudSpacing;
    private Quaternion HudRotation;

    [Header("OTHER SETTINGS")]
    [SerializeField] private bool showGizmos;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem walkParticle;
    [SerializeField] private ParticleSystem jumpParticle;
    [SerializeField] private ParticleSystem dashParticle;
    
    [Header("Audio Related Logic")]
    private AudioSource playerAudioSource;
    [SerializeField] private AudioClip playerDash;
    [SerializeField] private AudioClip playerJump;
    [SerializeField] private AudioClip collectArrowSound;
    [SerializeField] private AudioClip collectAbilitySound;
    [SerializeField] private AudioClip playerDieSound;

    [Header("Ability UI")]
    [SerializeField] public GameObject TimeStopAbilityUI;
    [SerializeField] public GameObject InvisibleAbilityUI;


    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _myCharacterSprite;
    private Collider2D thisPlayerCollider;

    private float angleAim;
   
    private bool isDashing;
    private bool isMoving;
    private bool isWallSliding;
    private bool isAiming;
    private bool canUseDash;
    private bool canJump;
    
    public bool IsMovementStop { get; set; } = false;
    public bool IsPlayerInvisible { get; set; } = false;
    public bool StopShoot { get; set; } = false;

    private Vector2 storedPlayerVelocity;

    private LayerMask groundLayerMask;
    private LayerMask arrowLayerMask;

    private InputManager inputManager;
    public InputManager GetInput => inputManager;

    private Player player;
    private Invisible invisibleScript;
    private TimeStop timeStopScript;
    

    private Stack<ArrowType> arrowStack;


    readonly float MOVEMENT_ENABLE_TIME = 0.2f;
    readonly int START_ARROW_COUNT = 3;

    public bool Grounded { get; set; } = true;
    public bool LeftHit { get; set; } = true;
    public bool RightHit { get; set; } = true;
    public int AbilityCount { get; set; } = 0;
    public void Initialize(int playerId)
    {
        LoadAlltheArrowHUD();
        this.playerId = playerId;
        InitializePlayersById();
        playerAudioSource = GetComponent<AudioSource>();
        InitializeArrowStack();
        InitializeReferences();
        isMoving = true;
        canUseDash = true;
        HudRotation = arrowHudParent.transform.rotation;
    }





    #region INITIALIZATION CODE
    private void InitializeReferences()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        thisPlayerCollider = gameObject.GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        groundLayerMask = LayerMask.GetMask("Ground");
        arrowLayerMask = LayerMask.GetMask("Arrow");
        invisibleScript = GetComponent<Invisible>();
        timeStopScript = GetComponent<TimeStop>();
        invisibleScript.inputManager = this.inputManager;
        timeStopScript.inputManager = this.inputManager;
    }

    private void LoadAlltheArrowHUD()
    {
        ArrowType[] alltypes = (ArrowType[])System.Enum.GetValues(typeof(ArrowType));
      
        for (int i = 0; i < alltypes.Length; i++)
        {
            AllArrowHUDSDictionary.Add(alltypes[i], Resources.Load<GameObject>("Prefabs/ArrowHUDS/" + alltypes[i].ToString() + "ArrowHUD"));
        }
       

    }

    private void InitializeArrowStack()
    {
        arrowStack = new Stack<ArrowType>();

        for (int i = 0; i < START_ARROW_COUNT; i++)
        {
            arrowStack.Push(ArrowType.NORMAL);
            AddNewArrowTOHUD(ArrowType.NORMAL);
        }


    }

    private void InitializePlayersById()
    {
        player = ReInput.players.GetPlayer(playerId);
        inputManager = new InputManager(this.player);

        Transform[] spawnPositions = MapManager.Instance.GetCurrentMapsSpawnPositions;
        transform.position = spawnPositions[playerId].position;
    }
    #endregion

    public void UpdateUnit()
    {
        if (IsMovementStop || GameManager.Instance.IsPaused) return;

        Grounded = isGrounded();
        LeftHit = isLeftHit();
        RightHit = isRightHit();


        Jump();
        Rotate();
        Dash();
        Aim();
        WallSlide();
    }
    private void LateUpdate()
    {
        arrowHudParent.transform.rotation = HudRotation;
    }
    public void FixedUpdateUnit()
    {
        if (IsMovementStop) return;
        Move();
    }


    #region MOVEMENT MECHANICS
    private void Move()
    {
        if (inputManager.HorizontalInput == 0 && !isWallSliding)
        {
            SetBoolForAnimation(_animator, true, false, false, false, false);
        }

        if (!isAiming)
        {
            if (isMoving)
            {
                _rb.velocity = new Vector2(inputManager.HorizontalInput * movementSpeed, _rb.velocity.y);
                if (inputManager.HorizontalInput != 0 && isGrounded())
                {
                    PlayParticle(walkParticle);

                    SetBoolForAnimation(_animator, false, true, false, false, false);
                }
            }

            if (isDashing)
            {
                _rb.velocity = new Vector2(inputManager.HorizontalInput * dashSpeed, _rb.velocity.y);
            }
        }
    }

    private void Rotate()
    {
        if (IsMovementStop) return;
        if (inputManager.HorizontalInput != 0 && !isWallSliding)
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
            if (isWallSliding)
            {
                if (LeftHit)
                    _rb.AddForce(new Vector2(wallJumpForce * wallJumpAngle.x, wallJumpForce * wallJumpAngle.y), ForceMode2D.Impulse);
                else if (RightHit)
                    _rb.AddForce(new Vector2(wallJumpForce * -wallJumpAngle.x, wallJumpForce * wallJumpAngle.y), ForceMode2D.Impulse);
            }
            else
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            PlayJumpSound();
            PlayParticle(jumpParticle);
            StopParticle(walkParticle);

            canJump = false;
        }

        if(!Grounded)
        {
            SetBoolForAnimation(_animator, false, false, true, false, false);
        }
    }
    void PlayJumpSound()
    {
        playerAudioSource.clip = playerJump;
        playerAudioSource.Play();
    }
    private void Dash()
    {
        if (inputManager.GetDashButtonDown && canUseDash && !isDashing && !isWallSliding)
        {
            StartDash();
            PlayParticle(dashParticle);
            PlayDashSound();
        }
    }
    void PlayDashSound()
    {
        playerAudioSource.clip = playerDash;
        playerAudioSource.Play();
    }
    private void StartDash()
    {
        isDashing = true;
        isMoving = false;
        if (TimeManager.Instance.IsTimeStopped)
        {
            TimeManager.Instance.AddDelegateRelatedToTime(() => StopDash(), maxDashTime, 1, true);
        }
        else
        {
            TimeManager.Instance.AddDelegate(() => StopDash(), maxDashTime, 1);
        }  
    }

    private void StopDash()
    {

        isDashing = false;
        isMoving = true;
        canUseDash = false;

        if (TimeManager.Instance.IsTimeStopped)
        {
            TimeManager.Instance.AddDelegateRelatedToTime(() => canUseDash = true, dashCooldown, 1, true);
        }
        else
        {
            TimeManager.Instance.AddDelegate(() => canUseDash = true, dashCooldown, 1);
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
            isAiming = false;
            _rb.velocity = new Vector2(_rb.velocity.x, -wallSlideSpeed);

            SetBoolForAnimation(_animator, false, false, false, true, false);
        }
        else
        {
            if (Grounded && _rb.velocity.y == 0 && !isMoving)
                isMoving = true;
        }
    }

    #endregion

    private void Aim()
    {
        if (StopShoot) return;
        if (inputManager.GetAimButton && !isWallSliding)
        {
            isAiming = true;

            if (_rb.velocity.x != 0) _rb.velocity = Vector2.zero;
        }

        if (inputManager.GetAimButtonUp && !isWallSliding)
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

            SetBoolForAnimation(_animator, false, false, false, false, true);
        }
        else
            handTransform.localEulerAngles = Vector2.zero;
    }

    private void AddNewArrowTOHUD(ArrowType ourArrowTYPE)
    {
        float countArrowSpacing = ((arrowStack.Count - 1) * arrowHudSpacing);
        GameObject arrowHud = Instantiate(AllArrowHUDSDictionary[ourArrowTYPE], new Vector2(arrowHudParent.transform.position.x - (countArrowSpacing), arrowHudParent.transform.position.y + arrowHudHeight), Quaternion.identity, arrowHudParent.transform);

        if (invisibleScript != null &&  invisibleScript.gameObject == true)
        {
            //invisibleScript.ChildSprites.Add(arrowHud.GetComponent<SpriteRenderer>());
            invisibleScript.MakeGrabbedArrowInvisible(arrowHud);
        }
       
        //GameObject arrowHud = Instantiate(AllArrowHUDS[ourArrowTYPE], new Vector2(0f, 0), Quaternion.identity, arrowHudParent.transform);
    }
    private void RemoveArrowFromHUD(ArrowType ourArowType)
    {
        if (arrowHudParent.transform.childCount != 0)
        {
            Destroy(arrowHudParent.transform.GetChild(arrowHudParent.transform.childCount - 1).gameObject);
        }

    }
    private void Shoot()
    {
        if (arrowStack.Count > 0)
        {
            ArrowType arrowType = arrowStack.Pop();
            RemoveArrowFromHUD(arrowType);
            Arrow newArrow = ArrowManager.Instance.Fire(arrowType, fireFromPos.position, fireFromPos.rotation);
            newArrow.Oninitialize();
            newArrow.AddForceInDirection(fireFromPos.right);

            newArrow.IgnoreSelfCollision(thisPlayerCollider);

           
        }
    }

    #region ON COLLISION CODE
    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((groundLayerMask | 1 << collision.gameObject.layer) == groundLayerMask)
            TimeManager.Instance.AddDelegate(() => isMoving = true, MOVEMENT_ENABLE_TIME, 1);
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

    public void EquipArrow(ArrowType toEquip, int equipCount)
    {
        PlayArrowEquipSound();
        for (int i = 0; i < equipCount; i++)
        {
            arrowStack.Push(toEquip);
            AddNewArrowTOHUD(toEquip);
        }
    }
    
    void PlayArrowEquipSound()
    {
        playerAudioSource.clip = collectArrowSound;
        playerAudioSource.Play();

    }
   public void EquipAbility(AbilitiesType toEquip)
   {
        
            PlayAbilityEquipSound();
            switch (toEquip)
            {
                case AbilitiesType.INVISIBLE:                  
                    invisibleScript.enabled = true;
                InvisibleAbilityUI.SetActive(true);

                    break;

                case AbilitiesType.TIME_STOP:
                    timeStopScript.enabled = true;
                    TimeStopAbilityUI.SetActive (true);
                    break;

                default:
                    Debug.LogError("Something went wrong while equiping Ability!");
                    break;

            }
         
   }
    void PlayAbilityEquipSound()
    {
        playerAudioSource.clip = collectAbilitySound;
        playerAudioSource.Play();
    }

    public void Freeze()
    {
        StopShoot = true;
        if (this.playerId != PlayerManager.Instance.PlayerIdUsedAbility)
        {
            storedPlayerVelocity = _rb.velocity;
            _rb.bodyType = RigidbodyType2D.Static;
            IsMovementStop = true;
        }
    }

    public void UnFreeze()
    {
        StopShoot = false;
        if (playerId != PlayerManager.Instance.PlayerIdUsedAbility)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.velocity = storedPlayerVelocity;
            IsMovementStop = false;

        }
    }

    public void StopEverythingAfterWin()
    {
        _rb.bodyType = RigidbodyType2D.Static;
        SetBoolForAnimation(_animator, false, false, false, false, false);
        StopShoot = true;
        IsMovementStop = true;
        timeStopScript.enabled = false;
        invisibleScript.enabled = false;
    }

    public void SetInvisibleScriptOff()
    {
        invisibleScript.enabled = false;
    }

    public void SetTimeStopScriptOff()
    {
        timeStopScript.enabled = false;
    }

    public void Die()
    {
        AudioSource.PlayClipAtPoint(playerDieSound, GameManager.Instance.MainCamera.transform.position, 0.34f);
        Destroy(gameObject);
    }

    private void PlayParticle(ParticleSystem particleSystem)
    {
        particleSystem.Play();
    }

    private void StopParticle(ParticleSystem particleSystem)
    {
        particleSystem.Pause();
        particleSystem.Clear();
    }

    private void SetBoolForAnimation(Animator myAnimator, bool isIdle, bool isWalking, bool isJumping, bool isSliding, bool isAim)
    {
        myAnimator.SetBool("isIdle", isIdle);
        myAnimator.SetBool("isWalking", isWalking);
        myAnimator.SetBool("isJumping", isJumping);
        myAnimator.SetBool("isSliding", isSliding);
        myAnimator.SetBool("isAim", isAim);
    }
}
