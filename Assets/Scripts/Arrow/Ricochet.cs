using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : Arrow
{
    //Get The Current Velocity
    Vector3 lastVelocity;
    public float DestroyAfterTimer;
   
    public override void Oninitialize()
    {
        base.Oninitialize();
        RB2D.gravityScale = 0.0f;
        Invoke("DestroyRicochetArrow", DestroyAfterTimer);
    }
    public override void OnUpdate()
    {
        lastVelocity = RB2D.velocity;
    }
    public override void ArrowRotation()
    {
        //Meant To be Empty
    }
   
    public override void OnHit(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Destroy(collision.gameObject);
            DestroyRicochetArrow();
        }
        else
        {
            float speed = lastVelocity.magnitude;
            Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            RB2D.velocity = direction * speed;
            float angle = Mathf.Atan2(RB2D.velocity.y, RB2D.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public override void DestroyArrow()
    {
        ArrowManager.Instance.DestroyArrow(this);
        Destroy(gameObject);
    }

    public void DestroyRicochetArrow()
    {
        DestroyArrow();
    }
}
