using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : Arrow
{
    public override void OnHit(Collision2D collision)
    {
        base.OnHit(collision);
        if (collision.gameObject.CompareTag("Wall"))
        {
            
            Stuck();
           
        }
    
    }
    
   private void Stuck()
    {
        hasHit = true;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        
    }
}
