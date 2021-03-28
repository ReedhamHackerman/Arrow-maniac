﻿using Rewired;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    [Header("Player 1 UI")]
    public RawImage p1_SelectedCharacterImg;
    public GameObject p1_confirmImage;

    [Header("Player 2 UI")]
    public RawImage p2_SelectedCharacterImg;
    public GameObject p2_confirmImage;

    private Dictionary<int, UIInputManager> playerInputs = new Dictionary<int, UIInputManager>();

    private Texture[] allCharacterTextures;

    public static int p1_CurrentSelectedId;
    public static int p2_CurrentSelectedId;

    public static Dictionary<int, int> playerWithSelectedCharacter = new Dictionary<int, int>();

    private int connectedPlayers;
    public int confirmedCount;

    void Start()
    {
        InitializeAllConnectedPlayers();
        InitializeAllCharacterImages();
    }

    void Update()
    {
        RefreshInput();
    }

    private void InitializeAllConnectedPlayers()
    {
        connectedPlayers = ReInput.controllers.joystickCount;

        if (connectedPlayers > 0)
        {
            for (int i = 0; i < connectedPlayers; i++)
            {
                Player p = ReInput.players.GetPlayer(i);
                UIInputManager input = new UIInputManager(p);

                playerInputs.Add(i, input);
            }
        }
    }

    private void RefreshInput()
    {
        foreach (KeyValuePair<int, UIInputManager> input in playerInputs)
        {
            if (input.Value.GetPreviousButtonDown)
            {
                ChangePrevioustById(input.Key);
            }

            if (input.Value.GetNextButtonDown)
            {
                ChangeNextById(input.Key);
            }

            if(input.Value.GetConfirmButtonDown)
            {
                ConfirmSelection(input.Key);
            }

            if (input.Value.GetStartButtonDown)
            {
                StartGame();
            }
        }
    }

    private void ChangeNextById(int playerId)
    {
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
        switch (playerId)
        {
            case 0:
                playerWithSelectedCharacter.Add(playerId, p1_CurrentSelectedId);
                p1_confirmImage.SetActive(true);
                break;

            case 1:
                playerWithSelectedCharacter.Add(playerId, p2_CurrentSelectedId);
                p2_confirmImage.SetActive(true);
                break;

            default:
                break;
        }

        confirmedCount++;
    }

    private void CancelSelection()
    {

    }

    private void StartGame()
    {
        if (confirmedCount == connectedPlayers)
        {
            SceneManager.LoadScene("MainScene");
        }
        else
            Debug.Log("All players must confirm!");
    }
}
