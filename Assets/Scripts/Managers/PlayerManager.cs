using Rewired;
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

    public List<PlayerUnit> PlayerUnitList { get; private set; }
    public int playerIdUsedAbility;

    private Dictionary<int, PlayerUnit> unitDictionary = new Dictionary<int, PlayerUnit>();

    private GameObject playerSpawnParent;

    public void Initialize()
    {
        GetPlayerCountAndInitialize();
    }
    public void Start()
    {

    }

    public void Refresh()
    {
        foreach (KeyValuePair<int, PlayerUnit> p in unitDictionary)
            p.Value.UpdateUnit();
    }

    public void FixedRefresh()
    {
        foreach (KeyValuePair<int, PlayerUnit> p in unitDictionary)
            p.Value.FixedUpdateUnit();
    }

    private void GetPlayerCountAndInitialize()
    {
        int connectedPlayerCount = ReInput.controllers.joystickCount;

        playerSpawnParent = new GameObject("Players Parent");

        if (connectedPlayerCount > 0)
        {
            for (int i = 0; i < connectedPlayerCount; i++)
            {
                int characterId = CharacterSelection.playerWithSelectedCharacter[i] + 1; //Added 1 because Player prefab names start with 1
                PlayerUnit playerUnit = GameObject.Instantiate<PlayerUnit>(Resources.Load<PlayerUnit>("Prefabs/Players/Player"+ characterId)); //Static for now Change this later
                playerUnit.Initialize(i);
                unitDictionary.Add(i, playerUnit);

                playerUnit.transform.SetParent(playerSpawnParent.transform);
            }
        }
        else
            Debug.LogError("Please connect a Joystick!");
    }

    public void PlayerDied(int id)
    {
        //Need to implement this
        PlayerUnit unit = unitDictionary[id];
        unitDictionary.Remove(id);
        unit.Die();
    }
}
