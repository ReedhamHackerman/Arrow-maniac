using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Singleton
    private InputManager() { }
    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new InputManager();
            }
            return _instance;
        }
    }
    #endregion
    public void Initialize()
    {

    }
    public void Start()
    {

    }
    public void Refresh()
    {

    }
    public void FixedRefresh()
    {

    }
}
