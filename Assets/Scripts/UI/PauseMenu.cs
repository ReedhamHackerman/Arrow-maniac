using Rewired;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseUIObj;
    [SerializeField] private GameObject pauseButtonsParent;
    [SerializeField] private float canPauseAfterTime = 2f;

    [Header("Loading UI")]
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private Image loadingFillbar;
    [SerializeField] private Text percentageText;

    private RoundSystemUI roundSystemUI;

    private RawImage[] selectorImages;
    private Dictionary<int, PlayerUnit> players = new Dictionary<int, PlayerUnit>();

    private bool canPause = false;
    
    private int currentSelectedBtnId;

    public void Initialize(Dictionary<int, PlayerUnit> playerDict)
    {
        this.players = playerDict;
        roundSystemUI = GetComponent<RoundSystemUI>();

        InitializeAllRawImages();

        TimeManager.Instance.AddDelegate(() => canPause = true, canPauseAfterTime, 1);
    }

    private void Update()
    {
        RefreshInput();
    }

    private void RefreshInput()
    {
        RefreshPlayerInputs();
    }

    private void RefreshPlayerInputs()
    {
        if(players.Count > 0 && canPause)
        {
            foreach (var player in players)
            {
                if(player.Value.GetInput.GetPauseButtonDown)
                {
                    ManagePauseMenuUI();
                }

                if (player.Value.GetInput.GetGoDownButtonDown)
                {
                    ManageSelector(true);
                }

                if (player.Value.GetInput.GetGoUpButtonDown)
                {
                    ManageSelector(false);
                }

                if (player.Value.GetInput.IsSelectButtonPressed)
                {
                    OnClickMenuButton();
                }
            }
        }
    }

    private void ManagePauseMenuUI()
    {
        if(Time.timeScale == 0)
        {
            GameManager.Instance.IsPaused = false;
            pauseUIObj.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            GameManager.Instance.IsPaused = true;
            pauseUIObj.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void ManageSelector(bool isDown)
    {
        if(isDown)
        {
            selectorImages[currentSelectedBtnId].enabled = false;

            currentSelectedBtnId++;
            currentSelectedBtnId = currentSelectedBtnId >= selectorImages.Length ? 0 : currentSelectedBtnId;

            selectorImages[currentSelectedBtnId].enabled = true;
        }
        else
        {
            selectorImages[currentSelectedBtnId].enabled = false;

            currentSelectedBtnId--;
            currentSelectedBtnId = currentSelectedBtnId < 0 ? (selectorImages.Length - 1) : currentSelectedBtnId;

            selectorImages[currentSelectedBtnId].enabled = true;
        }
    }

    private void InitializeAllRawImages()
    {
        selectorImages = new RawImage[pauseButtonsParent.transform.childCount];
        for (int i = 0; i < selectorImages.Length; i++)
        {
            selectorImages[i] = pauseButtonsParent.transform.GetChild(i).GetComponent<RawImage>();
            selectorImages[i].enabled = false;
        }

        currentSelectedBtnId = 0;
        selectorImages[currentSelectedBtnId].enabled = true;
    }

    private void OnClickMenuButton()
    {
        if(GameManager.Instance.IsPaused)
        {
            switch (currentSelectedBtnId)
            {
                case 0:
                    ManagePauseMenuUI();
                    break;

                case 1:
                    GameManager.Instance.IsPaused = false;
                    roundSystemUI.PlayAgain();
                    Time.timeScale = 1;
                    break;

                case 2:
                    GameManager.Instance.IsPaused = true;
                    LoadMainMenu();
                    Time.timeScale = 1;
                    break;

                case 3:

                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif

                    break;

                default:
                    break;
            }
        }
    }

    #region LOADING UI REGION
    private void LoadMainMenu()
    {
        loadingUI.SetActive(true);
        LoadLevel("Menu");
    }

    private void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelAsync(levelName));
    }

    private IEnumerator LoadLevelAsync(string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress);

            progress = progress >= 0.9f ? 1 : progress;
            loadingFillbar.fillAmount = Mathf.Clamp01(progress);

            percentageText.text = (progress * 100f).ToString("F0") + "%";

            yield return null;
        }
    }
    #endregion
}
