﻿using System.Collections;
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

    public virtual void ArrowRotation()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnHitWall(collision);
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
    
    public virtual void OnHitWall(Collision2D collision)
    {
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
