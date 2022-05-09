using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triple : Normal
{
    public override void IgnoreSelfCollision(Collider2D playerCollider)
    {
        Physics2D.IgnoreCollision(playerCollider, selfCollider2D, true);

        TimeManager.Instance.AddDelegate(() => Physics2D.IgnoreCollision(playerCollider, selfCollider2D, false), 2f, 1);


    }
    public override void OnHit(Collision2D collision)
    {
        base.OnHit(collision);
        if (collision.gameObject.CompareTag("Ground"))
        {
            ArrowManager.Instance.DestroyArrow(this);
        }

    }
}
