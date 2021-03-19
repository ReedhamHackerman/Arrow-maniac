﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Arrow
{
   
    //Specify the time That Arrow Should Stuck in the Object
    public float explodeAfterTimer;
    //Specify the Explosion Radius For The AOE Damage In That Area
    public float explosionRadius;
    public override void OnHit(Collision2D collision)
    {
        Stuck();
    }
   
    private void Stuck()
    {
        HasHit = true;
        RigidBody2D.velocity = Vector2.zero;
        RigidBody2D.isKinematic = true;
        Invoke("Explode", explodeAfterTimer);
        //Dont Make Delegate And Use Just Add function in this Way So U will not face Any initialisation Issue
        //TimeManager.Instance.AddDelegate(() => Explode(),explodeAfterTimer,1);
    }
   
    private void Explode()
    {
       
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, explosionRadius,LayerMask.GetMask("Player"));
        for (int i = 0; i < collider2Ds.Length; i++)
        {
            Destroy(collider2Ds[i].gameObject);
        }
        Destroy(this.gameObject);
    }
   
   
}
