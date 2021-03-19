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
        RigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        RigidBody2D.gravityScale = 0.0f;
    }
    public override void OnUpdate()
    {
        lastVelocity = RigidBody2D.velocity;
        Invoke("DestroyRicochetArrow", DestroyAfterTimer);
    }
    public override void ArrowRotation()
    {
        //Meant To be Empty
    }
   
    public override void OnHit(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
        }
        float speed = lastVelocity.magnitude;
        Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        RigidBody2D.velocity = direction * speed;
        float angle = Mathf.Atan2(RigidBody2D.velocity.y, RigidBody2D.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    public void DestroyRicochetArrow()
    {
        if (gameObject)
            Destroy(this.gameObject);
          
    }
}
