using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Invisible : Abilities
{
    private SpriteRenderer[] childSprites;
    private float fade = 1f;

    protected override void Initialize()
    {
        abilityTime = 2f;
        childSprites = GetComponentsInChildren<SpriteRenderer>();

    }

    protected override void Refresh()
    {
        if (inputManager.UseAbility /*&& canUseAbility*/)
        {
            canUseAbility = false;
            StartCoroutine(InvisibleAbility());
        }
    }

    IEnumerator InvisibleAbility()
    {
        while(fade > 0)
        {
            fade -= Time.deltaTime;
            FadeAnimation();
            yield return null;
        }

        yield return new WaitForSeconds(abilityTime);
        
        while (fade < 1)
        {
            fade += Time.deltaTime;
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
}
