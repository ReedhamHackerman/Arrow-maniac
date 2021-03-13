using UnityEngine;
using Rewired;

public class PlayerManager 
{
    #region Singleton
    private PlayerManager() { }
    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerManager();
            }
            return _instance;
        }
    }
    #endregion

    private PlayerUnit playerUnit;

    public void Initialize()
    {
        GetPlayerCountAndInitialize();
    }
    public void Start()
    {
        
    }

    public void Refresh()
    {
        playerUnit.UpdateUnit();
    }

    public void FixedRefresh()
    {
        playerUnit.FixedUpdateUnit();
    }

    private void GetPlayerCountAndInitialize()
    {
        int connectedPlayerCount = ReInput.controllers.joystickCount;

        for (int i = 0; i < connectedPlayerCount; i++)
        {
            playerUnit = GameObject.Instantiate<PlayerUnit>(Resources.Load<PlayerUnit>("Prefabs/Player1")); //Static for now Change this later
            playerUnit.Initialize(i);
        }
    }
}
