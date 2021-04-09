using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject indicationParentObj;
    [SerializeField] private GameObject btnParentObj;

    [SerializeField] private RawImage[] arrowIndications;
    [SerializeField] private Image[] btnImages;

    private UI_PlayerManager UI_PlayerManager;

    private GameObject characterSelectionObj;

    private int currentSelectedOption;

    public void InitializeMainMenu(UI_PlayerManager UI_PlayerManager, GameObject characterSelectionObj)
    {
        Cursor.visible = false;

        this.UI_PlayerManager = UI_PlayerManager;
        this.characterSelectionObj = characterSelectionObj;

        InitializeAndEnableImagesOnStart();
    }

    private void InitializeAndEnableImagesOnStart()
    {
        arrowIndications = indicationParentObj.GetComponentsInChildren<RawImage>();

        currentSelectedOption = 0;
        arrowIndications[currentSelectedOption].enabled = true;
    }

    public void Refresh()
    {
        if (gameObject.activeSelf)
        {
            foreach (KeyValuePair<int, UIInputManager> input in UI_PlayerManager.GetAllPlayersInput)
            {
                if (input.Value.GetGoDownButtonDown)
                {
                    ManageSelector(true);
                }

                if (input.Value.GetGoUpButtonDown)
                {
                    ManageSelector(false);
                }

                if (input.Value.GetMenuSelectButtonDown)
                {
                    OnOptionSelect();
                }
            }
        }
    }

    private void ManageSelector(bool isDown)
    {
        if(isDown)
        {
            arrowIndications[currentSelectedOption].enabled = false;

            currentSelectedOption++;
            currentSelectedOption = currentSelectedOption >= arrowIndications.Length ? 0 : currentSelectedOption;

            arrowIndications[currentSelectedOption].enabled = true;
        }
        else
        {
            arrowIndications[currentSelectedOption].enabled = false;

            currentSelectedOption--;
            currentSelectedOption = currentSelectedOption < 0 ? (arrowIndications.Length - 1) : currentSelectedOption;

            arrowIndications[currentSelectedOption].enabled = true;
        }
    }

    private void OnOptionSelect()
    {
        switch (currentSelectedOption)
        {
            case 0:
                characterSelectionObj.SetActive(true);
                gameObject.SetActive(false);
                break;

            case 1:

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
