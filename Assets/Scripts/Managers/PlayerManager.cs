using System.Collections;
using System.Collections.Generic;
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

    public List<PlayerUnit> PlayerUnitList { get; private set; }
    public int playerIdUsedAbility;

    public void Initialize()
    {
        GetPlayerCountAndInitialize();
    }
    public void Start()
    {
        
    }

    public void Refresh()
    {
        if(PlayerUnitList.Count > 0)
        {
            foreach (PlayerUnit player in PlayerUnitList)
                player.UpdateUnit();
        }
    }

    public void FixedRefresh()
    {
        if (PlayerUnitList.Count > 0)
        {
            foreach (PlayerUnit player in PlayerUnitList)
                player.FixedUpdateUnit();
        }
    }

    private void GetPlayerCountAndInitialize()
    {
        int connectedPlayerCount = ReInput.controllers.joystickCount;
        PlayerUnitList = new List<PlayerUnit>();

        if (connectedPlayerCount > 0)
        {
            for (int i = 0; i < connectedPlayerCount; i++)
            {
                PlayerUnit playerUnit = GameObject.Instantiate<PlayerUnit>(Resources.Load<PlayerUnit>("Prefabs/Players/Player1")); //Static for now Change this later
                playerUnit.Initialize(i);
                PlayerUnitList.Add(playerUnit);
            }
        }
        else
            Debug.LogError("Please connect a Joystick!");
    }
}
