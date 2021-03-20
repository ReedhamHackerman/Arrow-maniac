using UnityEngine;
using Rewired;
public enum ArrowType
{
    RICOCHET,
    NORMAL,
    EXPLOSIVE
}
public class Arrow : MonoBehaviour,IFreezable
{
    public ArrowType arrowType;
    [HideInInspector] protected Rigidbody2D RigidBody2D { get; set; }
    [HideInInspector] protected bool HasHit { get; set; }

    protected Vector2 arrowValocity;
  
    public virtual void ArrowRotation()
    {
        if (HasHit==false)
        {
            float angle = Mathf.Atan2(RigidBody2D.velocity.y, RigidBody2D.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        OnHit(collision);
       
    }
    
    public virtual void OnHit(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           //PlayerDie Logic Here And It should be Replaced By Player class Logic not by arrow 
            Destroy(collision.gameObject);
            //Destroying Game Object Without Any Particle Effects Later On That logic will be Changed
            Destroy(this.gameObject);
        }
    }
    public virtual void Oninitialize()
    {
        TimeManager.Instance.Initialize();
        RigidBody2D = GetComponent<Rigidbody2D>();
    }
    public virtual void OnUpdate()
    {
        if (HasHit == false)
        {
            ArrowRotation();
        }
        TimeManager.Instance.Refresh();
    }

    public virtual void Freeze()
    {
        arrowValocity = RigidBody2D.velocity;
        RigidBody2D.bodyType = RigidbodyType2D.Static;
    }

    public virtual void UnFreeze()
    {
        RigidBody2D.bodyType = RigidbodyType2D.Dynamic;
        RigidBody2D.velocity = arrowValocity;
        
    }
}
