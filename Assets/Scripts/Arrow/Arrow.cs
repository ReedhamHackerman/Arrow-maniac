using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public enum ArrowType
{
    RICOCHET,
    NORMAL,
    EXPLOSIVE
}
public class Arrow : MonoBehaviour
{
    public ArrowType arrowType;
   [HideInInspector] public Rigidbody2D rb;
    public bool hasHit;
    public virtual void ArrowRotation()
    {
        if (hasHit==false)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
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
            Destroy(this.gameObject);
        }
    }
  
    public void Update()
    {
       ArrowRotation();
    }
    public void  Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        
    }
   
}
