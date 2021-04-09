using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    public GameObject indicationParentObj;
    public GameObject btnParentObj;

    public RawImage[] arrowIndications;
    public Image[] btnImages;

    private UI_PlayerManager UI_PlayerManager;

    private GameObject characterSelectionObj;

    private int currentSelectedOption;

    public void InitializeMainMenu(UI_PlayerManager UI_PlayerManager, GameObject characterSelectionObj)
    {
        this.UI_PlayerManager = UI_PlayerManager;
        this.characterSelectionObj = characterSelectionObj;

        InitializeAndEnableImagesOnStart();
    }

    private void InitializeAndEnableImagesOnStart()
    {
        arrowIndications = new RawImage[indicationParentObj.transform.childCount];
        for (int i = 0; i < arrowIndications.Length; i++)
        {
            arrowIndications[i] = indicationParentObj.transform.GetChild(i).GetComponent<RawImage>();
        }

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

            case 2:

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
