using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisible : Abilities, IFreezable
{
    public List<SpriteRenderer> ChildSprites { get; set; } = new List<SpriteRenderer>();
    private float fade = 1f;
    private bool canPerfomeFade = true;
    private bool canUseAbility = true;
    
    [SerializeField] private AudioClip invisiblityAudioClip;

    PlayerUnit player;

    protected override void Initialize()
    {
        abilityTime = 2f;
        ChildSprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
        player = gameObject.GetComponent<PlayerUnit>();
    }

    protected override void Refresh()
    {
        if (inputManager.UseAbility && canUseAbility && !TimeManager.Instance.IsTimeStopped)    
        { 
            AudioSource.PlayClipAtPoint(invisiblityAudioClip, Camera.main.transform.position, 1f);
           
            StartCoroutine(InvisibleAbility());
            canUseAbility = false;
            player.InvisibleAbilityUI.SetActive(false);
            player.IsPlayerInvisible = true;
            player.StopShoot = true;
          
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
       
            while(fade < 1)
            {
            if (fade > 0.98f)
            {
                fade = 1f;
                FadeAnimation();
                ResetInvisibleAbility();
            }

                fade += (canPerfomeFade)? Time.deltaTime : 0;
                FadeAnimation();
                yield return null;
            }
    }

    private void FadeAnimation()
    {
        foreach (SpriteRenderer spriteRenderer in ChildSprites)
        {
            if(spriteRenderer != null)
                spriteRenderer.material.SetFloat("_Fade", fade);
        }

    }

    private void ResetInvisibleAbility()
    {
        player.StopShoot = false;
        player.AbilityCount = 0;
        player.IsPlayerInvisible = false;
        canUseAbility = true;
        player.SetInvisibleScriptOff();
    }

    public void MakeGrabbedArrowInvisible(GameObject gameObjToInvisible)
    {
        SpriteRenderer sr = gameObjToInvisible.GetComponent<SpriteRenderer>();
        sr.material.SetFloat("_Fade", fade);
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
