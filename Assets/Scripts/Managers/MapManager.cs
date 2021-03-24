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

    private MainMap[] loadedMaps;
    private MainMap currentMainMap;

    public void Initialize()
    {
        InitializeAndLoadMaps();
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

    private void InitializeAndLoadMaps()
    {
        loadedMaps = Resources.LoadAll<MainMap>("Prefabs/Maps/");

        int _random = Random.Range(0, loadedMaps.Length);
        currentMainMap = GameObject.Instantiate(loadedMaps[_random]);
    }

    public Transform[] GetCurrentMapsSpawnPositions()
    {
        return currentMainMap.PlayersPositions;
    }
}
