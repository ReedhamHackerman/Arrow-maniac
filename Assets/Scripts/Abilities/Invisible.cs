using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Invisible : Abilities,IFreezable
{
    private SpriteRenderer[] childSprites;
    private float fade = 1f;
    private bool canPerfomeFade;
    
    protected override void Initialize()
    {
        abilityTime = 2f;
        childSprites = GetComponentsInChildren<SpriteRenderer>();
        canPerfomeFade = true;

    }

    protected override void Refresh()
    {
        if (inputManager.UseAbility2/* && canUseAbility*/)
              
        {
            //canUseAbility = false;
            
            StartCoroutine(InvisibleAbility());
        }
    }

    IEnumerator InvisibleAbility()
    {
        
            while(fade >= 0)
            {
                fade -= (canPerfomeFade) ? Time.deltaTime : 0;
                FadeAnimation();
                yield return null;
            }
        

        yield return new WaitForSeconds(abilityTime);
   
       
            while(fade <= 1)
            {
                fade += (canPerfomeFade)? Time.deltaTime : 0;
                FadeAnimation();
                yield return null;
            }
       
    }

    private void FadeAnimation()
    {
        foreach (SpriteRenderer spriteRenderer in childSprites)
        {
            spriteRenderer.material.SetFloat("_Fade", fade);
        }
        
    }

    void IFreezable.Freeze()
    {
        canPerfomeFade = false;
    }

    void IFreezable.UnFreeze()
    {
        canPerfomeFade = true;
    }
}
