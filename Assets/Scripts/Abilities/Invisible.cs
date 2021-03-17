using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class Invisible : MonoBehaviour
{
    [SerializeField] Material disolveMat;
    [SerializeField] private float startTime;
    [SerializeField] private float invisibleTime = 2f;
    [SerializeField] private float endTime;
    [SerializeField] private bool canUseAbility = true;
    private float fade = 1f;

   
    void Start()
    {
        disolveMat = gameObject.GetComponent<SpriteRenderer>().material;
        
    }

   
    void Update()
    {

      
        if (Input.GetButtonDown("Jump") && canUseAbility)
        {
            canUseAbility = false;
            StartCoroutine(InvisibleAbility());
            
        }     
    }


    IEnumerator InvisibleAbility()
    {
        while(fade> 0)
        {
            fade -= Time.deltaTime;
            disolveMat.SetFloat("_Fade", fade);
            yield return null;
        }

        yield return new WaitForSeconds(invisibleTime);
        

        while (fade < 1)
        {
            fade += Time.deltaTime;
            disolveMat.SetFloat("_Fade", fade);
            yield return null;
        }

    }
}
