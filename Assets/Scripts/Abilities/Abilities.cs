using UnityEngine;


public enum AbilitiesType
{
    TIME_STOP,
    INVISIBLE
}

public abstract class Abilities : MonoBehaviour
{
   
    protected float abilityTime;
    public InputManager inputManager;
    void Start()
    {
        Initialize();
    }

    private void Update()
    {
        Refresh();
    }

    protected abstract void Refresh();

    protected abstract void Initialize();

}
