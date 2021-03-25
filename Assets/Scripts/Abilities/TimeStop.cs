using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System.Linq;
using UnityEngine.SceneManagement;

public class TimeStop : Abilities
{
    public bool isPlayerStopped;
    private PlayerUnit thisPlayerUnit;
    List< IFreezable> freezables = new List<IFreezable>();    
    protected override void Initialize()
    {     
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
        if (inputManager.UseAbility)
        {
            PlayerManager.Instance.playerIdUsedAbility = thisPlayerUnit.PlayerId;
            //StartCoroutine(TimeStopAbility());

            TimeManager.Instance.AddDelegate(() => Activate(), 0, 1);
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
