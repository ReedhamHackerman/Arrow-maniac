using UnityEngine;

public class MainScript : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    GameObject BackgroundMusic;
    private AudioSource mainBackGroundMusic;
    private void Awake()
    {
        InitializeBackgroundMusicAndPlay();
        GameManager.Instance.Initialize();
        UIManager.Instance.Initialize();
        CollectibleManager.Instance.Initialize();
        TimeManager.Instance.Initialize();
    }

    private void InitializeBackgroundMusicAndPlay()
    {
        BackgroundMusic = Instantiate( Resources.Load<GameObject>("Prefabs/BackgroundAudio/MainBackgroundAudio"),transform);
        mainBackGroundMusic = BackgroundMusic.GetComponent<AudioSource>();
        mainBackGroundMusic.Play();
    }

    private void Start()
    {
        GameManager.Instance.Start();
        UIManager.Instance.Start();
        CollectibleManager.Instance.Start();
    }
    private void Update()
    {
        GameManager.Instance.Refresh();
        UIManager.Instance.Refresh();
        CollectibleManager.Instance.Refresh();
        TimeManager.Instance.Refresh();
    }
    private void FixedUpdate()
    {
        GameManager.Instance.FixedRefresh();
        UIManager.Instance.FixedRefresh();
        CollectibleManager.Instance.FixedRefresh();
    }


}
