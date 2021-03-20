﻿using System.Collections;
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

    private ArrowFactory arrowFactory;

    public void Initialize()
    {
        arrowFactory = new ArrowFactory();
    }
    public void Start()
    {

    }

    public void Refresh()
    {
        arrowFactory?.RefreshUpdate();
    }

    public void FixedRefresh()
    {

    }

    public Arrow Fire(ArrowType arrowType, Vector2 pos, Quaternion rot)
    {
        Arrow toRet = arrowFactory.GetNewArrow(arrowType, pos, rot);
        return toRet;
    }

    public void DestroyArrow(Arrow toDestroy)
    {
        arrowFactory.RemoveFromList(toDestroy);
    }
}
