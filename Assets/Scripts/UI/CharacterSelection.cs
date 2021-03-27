using Rewired;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [Header("Player 1 UI")]
    public RawImage p1_SelectedCharacterImg;

    [Header("Player 2 UI")]
    public RawImage p2_SelectedCharacterImg;

    private Dictionary<int, InputManager> playerInputs = new Dictionary<int, InputManager>();

    private Texture[] allCharacterTextures;

    private int p1_CurrentSelectedId;
    private int p2_CurrentSelectedId;

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
        int connectedPlayerCount = ReInput.controllers.joystickCount;

        if (connectedPlayerCount > 0)
        {
            for (int i = 0; i < connectedPlayerCount; i++)
            {
                Player p = ReInput.players.GetPlayer(i);
                InputManager input = new InputManager(p);

                playerInputs.Add(i, input);
            }
        }
    }

    private void RefreshInput()
    {
        foreach (KeyValuePair<int, InputManager> input in playerInputs)
        {
            if (input.Value.GetPreviousButtonDown)
            {
                ChangePrevioustById(input.Key);
            }

            if (input.Value.GetNextButtonDown)
            {
                ChangeNextById(input.Key);
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
}
