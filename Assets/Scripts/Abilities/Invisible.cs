using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Invisible : Abilities
{
    [SerializeField] Material disolveMat;
    [SerializeField] private float startTime;
    [SerializeField] private float invisibleTime = 2f;
    [SerializeField] private float endTime;
    [SerializeField] private bool canUseAbility = true;

    private SpriteRenderer[] childSprites;

    private float fade = 1f;
    private Material material;
    private float fadeAnimationTime;
    private Material realmaterial;
    public InputManager inputManager;
    

    void Start()
    {

        childSprites = GetComponentsInChildren<SpriteRenderer>();

    }

   
    void Update()
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
            //material.SetFloat("_Fade", fadeEffectTime);
            foreach (SpriteRenderer spriteRenderer in childSprites)
            {
                spriteRenderer.material.SetFloat("_Fade", fade);
            }

            yield return null;
        }

        yield return new WaitForSeconds(invisibleTime);
        

        while (fade < 1)
        {
            fade += Time.deltaTime;
            foreach (SpriteRenderer spriteRenderer in childSprites)
            {
                spriteRenderer.material.SetFloat("_Fade", fade);
            }
            yield return null;
        }

    }
}
