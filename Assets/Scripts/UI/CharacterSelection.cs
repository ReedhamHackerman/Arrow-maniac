using Rewired;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CharacterSelection : MonoBehaviour
{
    [Header("Player 1 UI")]
    public RawImage p1_SelectedCharacterImg;
    public GameObject p1_confirmImage;

    [Header("Player 2 UI")]
    public RawImage p2_SelectedCharacterImg;
    public GameObject p2_confirmImage;

    [Header("Loading UI")]
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private Image loadingFillbar;
    [SerializeField] private Text percentageText;

    private Dictionary<int, UIInputManager> playerInputs = new Dictionary<int, UIInputManager>();
    private Dictionary<int, bool> playersIsCofirmed = new Dictionary<int, bool>();

    private Texture[] allCharacterTextures;


    [Header("SoundEffects")]
    private AudioSource pauseMenuAudioSource;
    [SerializeField] private AudioClip selectionSound;
    [SerializeField] private AudioClip playerChangeSound;

    private int p1_CurrentSelectedId;
    private int p2_CurrentSelectedId;

    public static Dictionary<int, int> playerWithSelectedCharacter;

    private int connectedPlayers;
    private int confirmedCount;

    private UI_PlayerManager UI_PlayerManager;

    public void InitializeCharacterSelection(UI_PlayerManager UI_PlayerManager)
    {
        this.UI_PlayerManager = UI_PlayerManager;
        InitializeAllConnectedPlayers();
        InitializeAllCharacterImages();
        pauseMenuAudioSource = GetComponent<AudioSource>();
    }

    private void InitializeAllConnectedPlayers()
    {
        playerWithSelectedCharacter = new Dictionary<int, int>();

        connectedPlayers = UI_PlayerManager.GetConnectedPlayers;

        if (connectedPlayers > 0)
        {
            for (int i = 0; i < connectedPlayers; i++)
            {
                playersIsCofirmed.Add(i, false);
                playerWithSelectedCharacter.Add(i, 0);
            }
        }
    }

    public void RefreshInput()
    {
        if (gameObject.activeSelf)
        {
            foreach (KeyValuePair<int, UIInputManager> input in UI_PlayerManager.GetAllPlayersInput)
            {
                if (input.Value.GetPreviousButtonDown)
                {
                    ChangePrevioustById(input.Key);
                }

                if (input.Value.GetNextButtonDown)
                {
                    ChangeNextById(input.Key);
                }

                if (input.Value.GetConfirmButtonDown)
                {
                    ConfirmSelection(input.Key);
                }

                if (input.Value.GetCancelButtonDown)
                {
                    CancelSelection(input.Key);
                }

                if (input.Value.GetStartButtonDown)
                {
                    CheckForAllPlayersAndStart();
                }
            }
        }
    }

    private void ChangeNextById(int playerId)
    {
        if (playersIsCofirmed[playerId]) return;
        AudioSource.PlayClipAtPoint(playerChangeSound, GameManager.Instance.MainCamera.transform.position);
        switch (playerId)
        {
            case 0:
                Next(ref p1_CurrentSelectedId, ref p1_SelectedCharacterImg);
                break;

            case 1:
                Next(ref p2_CurrentSelectedId, ref p2_SelectedCharacterImg);
                break;

            default:
                break;
        }
    }

    private void ChangePrevioustById(int playerId)
    {
        if (playersIsCofirmed[playerId]) return;
        AudioSource.PlayClipAtPoint(playerChangeSound, GameManager.Instance.MainCamera.transform.position);
        switch (playerId)
        {
            case 0:
                Previous(ref p1_CurrentSelectedId, ref p1_SelectedCharacterImg);
                break;

            case 1:
                Previous(ref p2_CurrentSelectedId, ref p2_SelectedCharacterImg);
                break;

            default:
                break;
        }
    }

    private void InitializeAllCharacterImages()
    {
        allCharacterTextures = Resources.LoadAll<Texture>("Prefabs/Characters");

        p1_CurrentSelectedId = 0;
        p2_CurrentSelectedId = 0;

        p1_SelectedCharacterImg.texture = allCharacterTextures[p1_CurrentSelectedId];
        p2_SelectedCharacterImg.texture = allCharacterTextures[p2_CurrentSelectedId];
    }

    private void Next(ref int characterId, ref RawImage characterRawImg)
    {
        characterId++;
        characterId = characterId >= allCharacterTextures.Length ? 0 : characterId;

        characterRawImg.texture = allCharacterTextures[characterId];
    }

    private void Previous(ref int characterId, ref RawImage characterRawImg)
    {
        characterId--;
        characterId = characterId < 0 ? (allCharacterTextures.Length - 1) : characterId;

        characterRawImg.texture = allCharacterTextures[characterId];
    }

    private void ConfirmSelection(int playerId)
    {
       
        if (!playersIsCofirmed[playerId])
        {
            switch (playerId)
            {
                case 0:
                    playerWithSelectedCharacter[playerId] = p1_CurrentSelectedId;
                    p1_confirmImage.SetActive(true);
                    break;

                case 1:
                    playerWithSelectedCharacter[playerId] = p2_CurrentSelectedId;
                    p2_confirmImage.SetActive(true);
                    break;

                default:
                    break;
            }
            AudioSource.PlayClipAtPoint(selectionSound, GameManager.Instance.MainCamera.transform.position);
            playersIsCofirmed[playerId] = true;
            confirmedCount++;
        }
    }

    private void CancelSelection(int playerId)
    {
        if (playersIsCofirmed[playerId])
        {
            switch (playerId)
            {
                case 0:
                    p1_confirmImage.SetActive(false);
                    break;

                case 1:
                    
                    p2_confirmImage.SetActive(false);
                    break;

                default:
                    break;
            }

            playerWithSelectedCharacter[playerId] = 0;
            playersIsCofirmed[playerId] = false;
            confirmedCount--;
        }
    }

    private void CheckForAllPlayersAndStart()
    {
        if (confirmedCount == connectedPlayers)
        {
            StartLoadingUI();
        }
        else
            Debug.Log("All players must confirm archers!");
    }

    #region LOADING UI REGION
    private void StartLoadingUI()
    {
        loadingUI.SetActive(true);
        LoadLevel("MainScene");
    }

    private void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelAsync(levelName));
    }

    private IEnumerator LoadLevelAsync(string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);

        while(!operation.isDone)
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
