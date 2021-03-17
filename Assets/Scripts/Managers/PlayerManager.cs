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

    private List<PlayerUnit> playerUnitList;

    public void Initialize()
    {
        GetPlayerCountAndInitialize();
    }
    public void Start()
    {
        
    }

    public void Refresh()
    {
        if(playerUnitList.Count > 0)
        {
            foreach (PlayerUnit player in playerUnitList)
                player.UpdateUnit();
        }
    }

    public void FixedRefresh()
    {
        if (playerUnitList.Count > 0)
        {
            foreach (PlayerUnit player in playerUnitList)
                player.FixedUpdateUnit();
        }
    }

    private void GetPlayerCountAndInitialize()
    {
        int connectedPlayerCount = ReInput.controllers.joystickCount;
        playerUnitList = new List<PlayerUnit>();

        if (connectedPlayerCount > 0)
        {
            for (int i = 0; i < connectedPlayerCount; i++)
            {
                PlayerUnit playerUnit = GameObject.Instantiate<PlayerUnit>(Resources.Load<PlayerUnit>("Prefabs/Player1")); //Static for now Change this later
                playerUnit.Initialize(i);
                playerUnitList.Add(playerUnit);
            }
        }
        else
            Debug.LogError("Please connect a Joystick!");
    }
}
