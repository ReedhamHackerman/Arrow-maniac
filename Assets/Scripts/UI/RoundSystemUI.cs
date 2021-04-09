using Rewired;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundSystemUI : MonoBehaviour
{
    public bool IsGameOver { get; set; } = false;
    public int WinScore { get; private set; } = 5;

    [SerializeField] private GameObject player1Parent;
    [SerializeField] private GameObject player2Parent;
    [SerializeField] private GameObject roundUI;
    [SerializeField] private GameObject[] player1trophies = new GameObject[5];
    [SerializeField] private GameObject[] player2trophies = new GameObject[5];

    [Header("Win Screen")]
    [SerializeField] private GameObject WinUI;
    [SerializeField] private GameObject winBorderParentObj;
    [SerializeField] private RawImage WonPlayerImage;
    [SerializeField] private Text winPlayerText;

    private RawImage[] winBtnBorderArr;

    private PauseMenu pauseMenu;

    /* [SerializeField] private GameObject player1Image;
     [SerializeField] private GameObject player2Image;*/

    private GameObject scoreTrophy; 
    private int trophySpawnDistance = 120;
    private int currentSelectedOption;

    private void Awake()
    {
        scoreTrophy = Resources.Load<GameObject>("Prefabs/HUD/Trophy");
        pauseMenu = GetComponent<PauseMenu>();

        LoadCharImageInUI();
        LoadWinUIBtnBorders();
    }

    void Start()
    {
        MakeAllTrophyDeactive();
        roundUI.SetActive(false);
        WinUI.SetActive(false);
    }

    void Update()
    {
        RefreshInputForWinScreen();
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
            if (i==0)
            {
                GameObject playerchar = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/HUD/CharImage"),player1Parent.transform.position , Quaternion.identity, roundUI.transform);
                RawImage playerImage = playerchar.GetComponent<RawImage>();
                playerImage.texture = Resources.Load<Texture2D>("Prefabs/Characters/" + charId);
            }
            else
            {
                GameObject playerchar = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/HUD/CharImage"), player2Parent.transform.position, Quaternion.identity, roundUI.transform);
                RawImage playerImage = playerchar.GetComponent<RawImage>();
                playerImage.texture = Resources.Load<Texture2D>("Prefabs/Characters/" + charId);
            }
        }
    }

    private void LoadWinUIBtnBorders()
    {
        winBtnBorderArr = winBorderParentObj.GetComponentsInChildren<RawImage>();
        winBtnBorderArr[0].enabled = true;
    }

    private void RefreshInputForWinScreen()
    {
        if (PlayerManager.Instance.UnitDictionary.Count > 0 && WinUI.activeSelf)
        {
            foreach (KeyValuePair<int, PlayerUnit> players in PlayerManager.Instance.UnitDictionary)
            {
                if (players.Value.GetInput.GetWinRightMoveDown)
                {
                    ManageButtonBorders(true);
                }

                if (players.Value.GetInput.GetWinLeftMoveDown)
                {
                    ManageButtonBorders(false);
                }

                if (players.Value.GetInput.GetWinOnOptionSelect)
                {
                    OnClickWinScreen();
                }
            }
        }
    }

    private void ManageButtonBorders(bool isRight)
    {
        if(isRight)
        {
            winBtnBorderArr[currentSelectedOption].enabled = false;

            currentSelectedOption++;
            currentSelectedOption = currentSelectedOption >= winBtnBorderArr.Length ? 0 : currentSelectedOption;

            winBtnBorderArr[currentSelectedOption].enabled = true;
        }
        else
        {
            winBtnBorderArr[currentSelectedOption].enabled = false;

            currentSelectedOption--;
            currentSelectedOption = currentSelectedOption < 0 ? (winBtnBorderArr.Length - 1) : currentSelectedOption;

            winBtnBorderArr[currentSelectedOption].enabled = true;
        }
    }

    private void OnClickWinScreen()
    {
        switch (currentSelectedOption)
        {
            case 0:
                pauseMenu.LoadLevel("Menu");
                break;

            case 1:
                PlayAgain();
                break;

            default:
                break;
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
                PlayerManager.Instance.ScoreDict[key] = PlayerManager.Instance.ScoreDict[key] + 1;
                playerUnit.StopEverythingAfterWin();
            }
        }
    }


    public void WinScreenUI()
    {
        foreach (KeyValuePair<int, int> dict in PlayerManager.Instance.ScoreDict)
        {
            if (dict.Value == WinScore)
            {
                int playerIDName = dict.Key + 1;  // adding 1 because player id start with 0
                winPlayerText.text = "P"+ playerIDName +" WON!"; 
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
