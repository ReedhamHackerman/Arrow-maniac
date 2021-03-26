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

    private CollectibleChest chestPrefab;
    private GameObject chestSpawnParent;

    public Transform[] GetCurrentMapsSpawnPositions => currentMainMap.PlayersPositions;

    public void Initialize()
    {
        InitializeMaps();
        
    }

    public void Start()
    {
        TimeManager.Instance.AddDelegate(() => SpawnChests(), currentMainMap.ChestSpawnDuration, 1);
    }

    public void Refresh()
    {

    }

    public void FixedRefresh()
    {

    }

    private void InitializeMaps()
    {
        loadedMaps = Resources.LoadAll<MainMap>("Prefabs/Maps/");
        chestPrefab = Resources.Load<CollectibleChest>("Prefabs/CollectibleChests/ChestPrefab");

        chestSpawnParent = new GameObject("Collectible Chests");

        int _random = Random.Range(0, loadedMaps.Length);
        _random = 0; ////
        currentMainMap = GameObject.Instantiate(loadedMaps[_random]);
    }

    private void SpawnChests()
    {
        for (int i = 0; i < currentMainMap.ChestPositions.Length; i++)
        {
            CollectibleChest chest = GameObject.Instantiate(chestPrefab, currentMainMap.ChestPositions[i].position, Quaternion.identity);
            chest.transform.SetParent(chestSpawnParent.transform);
            chest.Initialize();
        }
    }
}
