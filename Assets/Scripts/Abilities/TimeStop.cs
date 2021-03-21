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
            StartCoroutine(TimeStopAbility());
        }
    }

    IEnumerator TimeStopAbility()
    {
        freezables = Finds<IFreezable>();

        foreach (IFreezable iFreezable in freezables)
        {
            iFreezable.Freeze();
        }
      
        yield return new WaitForSeconds(abilityTime);

        foreach (IFreezable iFreezable in freezables)
        {
            iFreezable.UnFreeze();
        }

    }    
}
