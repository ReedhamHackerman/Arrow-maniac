using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : Arrow
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

    public override void Freeze()
    {
        if (HasHit) return; // so that stuck normal arrows do not get affected 
        base.Freeze();
        IsPickable = true;
    }

    public override void UnFreeze()
    {
        if (HasHit) return; // so that stuck normal arrows do not get affected 
        base.UnFreeze();
    }



}
