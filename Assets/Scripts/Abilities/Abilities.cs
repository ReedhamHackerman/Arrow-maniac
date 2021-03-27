using UnityEngine;

public abstract class Abilities : MonoBehaviour
{

    public enum AbilitiesType
    {
        TIME_STOP,
        INVISIBLE
    }

    [SerializeField] protected bool canUseAbility = true;
    [SerializeField] protected float abilityTime;
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
