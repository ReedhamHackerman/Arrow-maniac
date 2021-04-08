using Rewired;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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

    public int playerIdUsedAbility;

    public Dictionary<int, PlayerUnit> UnitDictionary { get; set; } = new Dictionary<int, PlayerUnit>();
    public Dictionary<int, int> ScoreDict { get; set; } = new Dictionary<int, int>();

    private GameObject playerSpawnParent;

    private RoundSystemUI roundSystemUI;
    private PauseMenu pauseMenuUI;

    public void Initialize()
    {
        GetPlayerCountAndInitialize();

        roundSystemUI = GameObject.FindObjectOfType<RoundSystemUI>();
        pauseMenuUI = GameHUD.FindObjectOfType<PauseMenu>();
    }
    public void Start()
    {
        pauseMenuUI.Initialize(UnitDictionary);
    }

    public void Refresh()
    {
        foreach (KeyValuePair<int, PlayerUnit> p in UnitDictionary)
            p.Value.UpdateUnit();
    }

    public void FixedRefresh()
    {
        foreach (KeyValuePair<int, PlayerUnit> p in UnitDictionary)
            p.Value.FixedUpdateUnit();
    }

    private void GetPlayerCountAndInitialize()
    {
        UnitDictionary.Clear();

        int connectedPlayerCount = ReInput.controllers.joystickCount;
        playerSpawnParent = new GameObject("Players Parent");

        if (connectedPlayerCount > 0)
        {
            for (int i = 0; i < connectedPlayerCount; i++)
            {
                int characterId = CharacterSelection.playerWithSelectedCharacter[i] + 1; //Added 1 because Player prefab names start with 1
                PlayerUnit playerUnit = GameObject.Instantiate<PlayerUnit>(Resources.Load<PlayerUnit>("Prefabs/Players/Player"+ characterId)); //Static for now Change this later
                playerUnit.Initialize(i);
                UnitDictionary.Add(i, playerUnit);
                AddPlayerInScoreDict(i,0);

                playerUnit.transform.SetParent(playerSpawnParent.transform);
            }
        }
        else
            Debug.LogError("Please connect a Joystick!");
    }

    public void AddPlayerInScoreDict(int id,int score)
    {
        if (UnitDictionary.Count > ScoreDict.Count)
        {
            ScoreDict.Add(id,score);
        }
    }

    public void PlayerDied(int id)
    {
        //Need to implement this
        PlayerUnit unit = UnitDictionary[id];
        UnitDictionary.Remove(id);
        TimeManager.Instance.AddDelegate(() => ActivateUI(), 0.1f, 1);
        unit.Die();
    }

    public void CheckForWin()
    {
        foreach (KeyValuePair<int, int> dict in ScoreDict)
        {
            if (dict.Value == roundSystemUI.WinScore)
            {
                roundSystemUI.WinScreenUI();
                roundSystemUI.IsGameOver = true;
            }
        }
    }

    public void ActivateUI()
    {
        if (UnitDictionary.Count == 1)
        {
            roundSystemUI.IncrementScore();
            CheckForWin();
            if (!roundSystemUI.IsGameOver)
            {
                ActivateRoundUI();
            }
        }
        else
        {
            ActivateRoundUI();
        }
        
    }

    public void ActivateRoundUI()
    {
        roundSystemUI.StartTrophyUI();
        roundSystemUI.IncrementTrophyInUI();

        TimeManager.Instance.AddDelegate(() => roundSystemUI.StopTrophyUI(), 4, 1);
        TimeManager.Instance.AddDelegate(() => roundSystemUI.ReloadScene(), 4, 1);
    }


}
