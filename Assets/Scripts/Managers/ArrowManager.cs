using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager 
{
    #region Singleton
    private ArrowManager() { }
    private static ArrowManager _instance;
    public static ArrowManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ArrowManager();
            }
            return _instance;
        }
    }
    #endregion
    GameObject ricoChetArrow;
    GameObject normalArrow;
    GameObject ExplosiveArrow;

    public void Initialize()
    {
        normalArrow = (GameObject)Resources.Load("Prefabs/Arrows/NormalArrow");
        ricoChetArrow = (GameObject)Resources.Load("Prefabs/Arrows/RicochetArrow");
        ExplosiveArrow = (GameObject)Resources.Load("Prefabs/Arrows/ExplosiveArrow");
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
