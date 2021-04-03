using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeStop : Abilities
{
    public bool isPlayerStopped;
    private PlayerUnit thisPlayerUnit;
    List< IFreezable> freezables = new List<IFreezable>();
    private bool canUseTimeStop = true;
    [SerializeField] private GameObject TimeStopAbilityUI;
    [SerializeField] private AudioClip timeStopAudioClip;
    protected override void Initialize()
    {
        TimeStopAbilityUI.SetActive(true);
        abilityTime = 2f;
        thisPlayerUnit = gameObject.GetComponent<PlayerUnit>();

    }

    public static List<T> Finds<T>()
    {
        List<T> interfaces = new List<T>();
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var rootGameObject in rootGameObjects)
        {
            T[] childrenInterfaces = rootGameObject.GetComponentsInChildren<T>();
            foreach (var childInterface in childrenInterfaces)
            {
                interfaces.Add(childInterface);
            }
        }
        return interfaces;
    }

    protected override void Refresh()
    {
        if (inputManager.UseAbility && canUseTimeStop )
        {
            AudioSource.PlayClipAtPoint(timeStopAudioClip, Camera.main.transform.position, 1.0f);
            PlayerManager.Instance.playerIdUsedAbility = thisPlayerUnit.PlayerId;
            //StartCoroutine(TimeStopAbility());
            TimeManager.Instance.AddDelegate(() => Activate(), 0, 1);
            canUseTimeStop = false;
            TimeStopAbilityUI.SetActive(false);
        }
    }

    private void Activate()
    {
        freezables = Finds<IFreezable>();
        TimeManager.Instance.IsTimeStopped = true;

        foreach (IFreezable iFreezable in freezables)
        {
            iFreezable.Freeze();
        }

        TimeManager.Instance.AddDelegateRelatedToTime(() => Deactivate(), abilityTime, 1, true);
        TimeManager.Instance.AddTimeToDelegateMethods(abilityTime);

    }

    private void Deactivate()
    {
        foreach (IFreezable iFreezable in freezables)
        {
            iFreezable.UnFreeze();
        }

        TimeManager.Instance.IsTimeStopped = false;
    }
}
