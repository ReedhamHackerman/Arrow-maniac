using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
   
    [SerializeField] private Camera mainCamera;
   
    private void Awake()
    {
        GameManager.Instance.Initialize();
        InputManager.Instance.Initialize();
        UIManager.Instance.Initialize();
        CollectibleManager.Instance.Initialize();
        
    }
    private void Start()
    {
        GameManager.Instance.Start();
        InputManager.Instance.Start();
        UIManager.Instance.Start();
        CollectibleManager.Instance.Start();
    }
    private void Update()
    {
        GameManager.Instance.Refresh();
        InputManager.Instance.Refresh();
        UIManager.Instance.Refresh();
        CollectibleManager.Instance.Refresh();
    }
    private void FixedUpdate()
    {
        GameManager.Instance.FixedRefresh();
        InputManager.Instance.FixedRefresh();
        UIManager.Instance.FixedRefresh();
        CollectibleManager.Instance.FixedRefresh();
    }
   
   
}
