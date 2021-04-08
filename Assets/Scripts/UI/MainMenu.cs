using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject indicationParentObj;
    public GameObject btnParentObj;

    public RawImage[] arrowIndications;
    public Image[] btnImages;

    private void Start()
    {
        InitializeAndEnableImagesOnStart();
    }

    private void InitializeAndEnableImagesOnStart()
    {
        arrowIndications = new RawImage[indicationParentObj.transform.childCount];
        for (int i = 0; i < arrowIndications.Length; i++)
        {
            arrowIndications[i] = indicationParentObj.transform.GetChild(i).GetComponent<RawImage>();
        }

        btnImages = new Image[btnParentObj.transform.childCount];
        for (int i = 0; i < btnImages.Length; i++)
        {
            btnImages[i] = btnParentObj.transform.GetChild(i).GetComponent<Image>();
        }


    }

    private void ManageSelector(bool isDown)
    {
        if(isDown)
        {

        }
        else
        {

        }
    }
}
