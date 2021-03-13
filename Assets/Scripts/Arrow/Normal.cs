using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal : Arrow
{
    public override void OnHitWall(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag ("Wall"))
        {
            Stuck();
            Debug.Log("Hello");
        }
    
    }
    
   private void Stuck()
    {
        rb.constraints = (RigidbodyConstraints2D)RigidbodyConstraints.FreezeAll;
    }
}
