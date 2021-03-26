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
        IsPickable = true;
        selfCollider2D.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsPickable)
        {
            PlayerUnit player = collision.gameObject.GetComponent<PlayerUnit>();
            player.EquipArrow(arrowType, 1);

            selfCollider2D.isTrigger = false;

            Debug.Log("EquipArrow");
            DestroyArrow();
        }
    }
}
