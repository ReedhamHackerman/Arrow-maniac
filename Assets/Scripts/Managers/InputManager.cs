using Rewired;

public class InputManager 
{
    private Player player;

    public float HorizontalInput => player?.GetAxis("Move Horizontal") ?? 0f;
    public float VerticalInput => player?.GetAxis("Move Vertical") ?? 0f;
    public bool GetJumpButtonDown => player?.GetButtonDown("Jump") ?? false;
    public bool GetDashButtonDown => player?.GetButtonDown("Dash") ?? false;
    public bool UseAbility => player?.GetButtonDown("Ability") ?? false;

    public bool UseAbility2 => player?.GetButtonDown("Ability2") ?? false;

    public bool GetAimButton => player?.GetButton("Aim") ?? false;
    public bool GetAimButtonUp => player?.GetButtonUp("Aim") ?? false;

    public InputManager(Player player)
    {
        this.player = player;
    }

    public void Initialize()
    {

    }

    public void Start()
    {

    }

    public void Refresh()
    {

    }

    public void FixedRefresh()
    {

    }
}
