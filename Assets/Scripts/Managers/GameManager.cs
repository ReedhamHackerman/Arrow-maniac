﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    #region Singleton
    private GameManager() { }
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }
    #endregion

    public void Initialize()
    {
        PlayerManager.Instance.Initialize();
        ArrowManager.Instance.Initialize();
        
    }
    public void Start()
    {
        PlayerManager.Instance.Start();
        ArrowManager.Instance.Start();

    }
    public void Refresh()
    {
        PlayerManager.Instance.Refresh();
        ArrowManager.Instance.Refresh();
    }
    public void FixedRefresh()
    {
        PlayerManager.Instance.FixedRefresh();
        ArrowManager.Instance.FixedRefresh();
    }
}
