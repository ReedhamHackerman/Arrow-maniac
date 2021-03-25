using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : Arrow, IFreezable
{
    public override void OnHit(Collision2D collision)
    {
        base.OnHit(collision);
        if (collision.gameObject.CompareTag("Ground"))
        {
            
            Stuck();
           
        }
    
    }
    
   private void Stuck()
   {
        HasHit = true;
        RB2D.velocity = Vector3.zero;
        RB2D.isKinematic = true;
        
    }


    public virtual void Freeze()
    {
        if (HasHit) return;

        arrowValocity = RB2D.velocity;

        RB2D.velocity = Vector3.zero;
        RB2D.isKinematic = true;
    }

    public virtual void UnFreeze()
    {
        if (HasHit) return;

        RB2D.velocity = arrowValocity;
        RB2D.isKinematic = false;
        isTimeStopped = false;
    }
}
