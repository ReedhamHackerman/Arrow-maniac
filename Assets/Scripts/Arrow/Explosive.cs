using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Arrow
{
    public float radius;
    public float stuckTime;
    public bool hasStuck = false;
    public override void OnHit(Collision2D collision)
    {
       
            Stuck(radius,stuckTime,collision);
        
    }

    private void Stuck()
    {
        hasHit = true;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        hasStuck = true;
    }

   

    private void Stuck(float radius,float stuckTime,Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Wall")||collision2D.gameObject.CompareTag("Player"))
        {
            Stuck();
           
        }
       
    }
    private new void Update()
    {
        if (hasStuck == true)
        {
            Explode();
        }
    }
    private void Explode()
    {
       
    }
    private void PlayerDie()
    {

    }
}
