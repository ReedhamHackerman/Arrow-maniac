using UnityEngine;

public class UI : MonoBehaviour
{
    UI_PlayerManager UI_PlayerManager;
    CharacterSelection characterSelection;

    private void Awake()
    {
        UI_PlayerManager = GetComponent<UI_PlayerManager>();
        characterSelection = GameObject.FindObjectOfType<CharacterSelection>();
    }

    private void Start()
    {
        UI_PlayerManager.InitializeAllConnectedPlayers();
        characterSelection.InitializeCharacterSelection(UI_PlayerManager);
    }

    private void Update()
    {
        characterSelection.RefreshInput();
    }
}
