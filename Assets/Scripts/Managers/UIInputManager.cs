using Rewired;

public class UIInputManager
{
    private Player player;

    public bool GetPreviousButtonDown => player?.GetButtonDown("Previous") ?? false;
    public bool GetNextButtonDown => player?.GetButtonDown("Next") ?? false;
    public bool GetConfirmButtonDown => player?.GetButtonDown("Confirm") ?? false;
    public bool GetCancelButtonDown => player?.GetButtonDown("Cancel") ?? false;
    public bool GetStartButtonDown => player?.GetButtonDown("Start") ?? false;

    public UIInputManager(Player player)
    {
        this.player = player;
    }
}
