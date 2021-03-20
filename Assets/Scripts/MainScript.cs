using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
   
    [SerializeField] private Camera mainCamera;
   
    private void Awake()
    {
        GameManager.Instance.Initialize();
        UIManager.Instance.Initialize();
        CollectibleManager.Instance.Initialize();
        TimeManager.Instance.Initialize();
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
