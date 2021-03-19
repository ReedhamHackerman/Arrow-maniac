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
        HasHit = true;
        RigidBody2D.velocity = Vector3.zero;
        RigidBody2D.isKinematic = true;
        
    }
}
