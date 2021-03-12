using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        playerUnit = GameObject.Instantiate<PlayerUnit>(Resources.Load<PlayerUnit>("Prefabs/Player"));
        playerUnit.Initialize();
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
