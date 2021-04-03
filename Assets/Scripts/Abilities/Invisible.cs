using System.Collections;
using UnityEngine;

public class Invisible : Abilities, IFreezable
{
    public SpriteRenderer[] childSprites;
    private float fade = 1f;
    private bool canPerfomeFade;
    private bool canUseAbility = true;
    [SerializeField] private GameObject invisibleAbilityUI;
    [SerializeField] private AudioClip invisiblityAudioClip;

    protected override void Initialize()
    {
        invisibleAbilityUI.SetActive(true);
        abilityTime = 2f;
        childSprites = GetComponentsInChildren<SpriteRenderer>();
        canPerfomeFade = true;

    }

    protected override void Refresh()
    {
        if (inputManager.UseAbility && canUseAbility)    
        { 
            AudioSource.PlayClipAtPoint(invisiblityAudioClip, Camera.main.transform.position, 1f);
           
            StartCoroutine(InvisibleAbility());
            canUseAbility = false;
            invisibleAbilityUI.SetActive(false);
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
            if(spriteRenderer != null)
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
