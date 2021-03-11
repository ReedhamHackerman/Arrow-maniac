using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager 
{
    #region Singleton
    private CollectibleManager() { }
    private static CollectibleManager _instance;
    public static CollectibleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CollectibleManager();
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
