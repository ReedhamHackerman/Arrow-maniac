using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager 
{
    #region Singleton
    private MapManager() { }
    private static MapManager _instance;
    public static MapManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MapManager();
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
