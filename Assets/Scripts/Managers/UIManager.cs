
public class UIManager
{
    #region Singleton
    private UIManager() { }
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }
    #endregion
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
