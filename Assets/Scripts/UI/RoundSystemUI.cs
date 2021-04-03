using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Rewired.Controller;
using System.Linq;

public class RoundSystemUI : MonoBehaviour
{
    [SerializeField] private GameObject player1Parent;
    [SerializeField] private GameObject player2Parent;
    [SerializeField] private GameObject roundUI;
    [SerializeField] private GameObject WinUI;
    [SerializeField] private RawImage WonPlayerImage;

    private GameObject[] player1trophies;
    private GameObject[] player2trophies;    
    private GameObject scoreTrophy; 
    private int trophySpawnDistance = 120;
    private int charImageSpawnDistance = 0;


    public bool IsGameOver { get; set; } = false;
    public int WinScore { get; private set; } = 5;
   

    private void Awake()
    {
        player1trophies = new GameObject[WinScore];
        player2trophies = new GameObject[WinScore];

        scoreTrophy = Resources.Load<GameObject>("Prefabs/HUD/Trophy");
        LoadCharImageInUI();
    }

    void Start()
    {
        LoadTrophiesinArray();
        MakeAllTrophyDeactive();
        roundUI.SetActive(false);
        WinUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void StopTrophyUI()
    {
        roundUI.SetActive(false);
    }

    public void StartTrophyUI()
    {
        roundUI.SetActive(true);
    }

    private void LoadCharImageInUI()
    {
        int connectedPlayerCount = ReInput.controllers.joystickCount;

        for (int i = 0; i < connectedPlayerCount; i++)
        {
            int charId = CharacterSelection.playerWithSelectedCharacter[i];
            GameObject playerchar =  GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/HUD/CharImage"),new Vector3 (player1Parent.transform.position.x,player1Parent.transform.position.y - charImageSpawnDistance,player1Parent.transform.position.z) ,Quaternion.identity,roundUI.transform);
            RawImage playerImage = playerchar.GetComponent<RawImage>();
            playerImage.texture = Resources.Load<Texture2D>("Prefabs/Characters/" + charId);
            charImageSpawnDistance += 245;
        }
    }

    public  void LoadTrophiesinArray()
    {
        for (int i = 0; i < WinScore; i++)
        {
            player1trophies[i] = Instantiate(scoreTrophy,new Vector3(player1Parent.transform.position.x + trophySpawnDistance, player1Parent.transform.position.y,player1Parent.transform.position.z), Quaternion.identity, player1Parent.transform);
            player2trophies[i] = Instantiate(scoreTrophy,new Vector3(player2Parent.transform.position.x + trophySpawnDistance, player2Parent.transform.position.y, player2Parent.transform.position.z), Quaternion.identity, player2Parent.transform);

            trophySpawnDistance += 120;
        }
    }

    public void MakeAllTrophyDeactive()
    {
        for (int i = 0; i < WinScore; i++)
        {
            player1trophies[i].SetActive(false);
            player2trophies[i].SetActive(false);
        }
    }

    public void IncrementScore()
    {
              
        if (PlayerManager.Instance.UnitDictionary.Count == 1)
        {
            foreach (int key in PlayerManager.Instance.UnitDictionary.Keys)
            {
                PlayerUnit playerUnit = PlayerManager.Instance.UnitDictionary[key];
                playerUnit.IsMovementStop = true;
                PlayerManager.Instance.ScoreDict[key] = PlayerManager.Instance.ScoreDict[key] + 1;
            }
        }
    }


    public void WinScreenUI()
    {
        foreach (KeyValuePair<int, int> dict in PlayerManager.Instance.ScoreDict)
        {
            if (dict.Value == WinScore)
            {
                int charId = CharacterSelection.playerWithSelectedCharacter[dict.Key];
                WonPlayerImage.texture = Resources.Load<Texture2D>("Prefabs/Characters/" + charId);
                WinUI.SetActive(true);
            }

        }
    }


    public void ReloadScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void IncrementTrophyInUI()
    {
        foreach (KeyValuePair<int, int> dict in PlayerManager.Instance.ScoreDict)
        {
            if (dict.Key == 0)
            {
                for (int i = 0; i < PlayerManager.Instance.ScoreDict[dict.Key]; i++)
                {
                    player1trophies[i].SetActive(true);
                }
            }

            if (dict.Key == 1)
            {
                for (int i = 0; i < PlayerManager.Instance.ScoreDict[dict.Key]; i++)
                {
                    player2trophies[i].SetActive(true);
                }
            }
        }
    }


    public void PlayAgain()
    {
        List<int> tempKeys = new List<int>(PlayerManager.Instance.ScoreDict.Keys);

        foreach (int key in tempKeys)
        {
            PlayerManager.Instance.ScoreDict[key] = 0; 
        }
        MakeAllTrophyDeactive();
        ReloadScene();
    }

}
