using UnityEngine;

public class UI : MonoBehaviour
{
    UI_PlayerManager UI_PlayerManager;
    CharacterSelection characterSelection;
    MainMenu mainMenu;

    private void Awake()
    {
        UI_PlayerManager = GetComponent<UI_PlayerManager>();
        characterSelection = GameObject.FindObjectOfType<CharacterSelection>();
        mainMenu = GameObject.FindObjectOfType<MainMenu>();

        EnableOrDisableUI();
    }

    private void Start()
    {
        UI_PlayerManager.InitializeAllConnectedPlayers();
        characterSelection.InitializeCharacterSelection(UI_PlayerManager);
        mainMenu.InitializeMainMenu(UI_PlayerManager, characterSelection.gameObject);
    }

    private void Update()
    {
        characterSelection.RefreshInput();
        mainMenu.Refresh();
    }

    private void EnableOrDisableUI()
    {
        mainMenu.gameObject.SetActive(true);
        characterSelection.gameObject.SetActive(false);
    }
}
