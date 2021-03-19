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

    private Transform mapParentObj;
    private GameObject[] allMaps;

    public void Initialize()
    {
        InitializeAllMaps();
        ActivateRandomMap();
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

    private void InitializeAllMaps()
    {
        mapParentObj = GameObject.Find("MapParent").transform;

        allMaps = new GameObject[mapParentObj.childCount];
        for (int i = 0; i < mapParentObj.childCount; i++)
            allMaps[i] = mapParentObj.GetChild(i).gameObject;
    }

    private void ActivateRandomMap()
    {
        int randomIndex = UnityEngine.Random.Range(0, allMaps.Length);
        allMaps[randomIndex].gameObject.SetActive(true);
    }
}
