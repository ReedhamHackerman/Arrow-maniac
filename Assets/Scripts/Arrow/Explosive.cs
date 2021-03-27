﻿using UnityEngine;

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
        RB2D.velocity = Vector2.zero;
        RB2D.isKinematic = true;
        TimeManager.Instance.AddDelegate(() => Explode(), explodeAfterTimer, 1);
    }

    private void Explode()
    {

        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, explosionRadius, LayerMask.GetMask("Player"));
        for (int i = 0; i < collider2Ds.Length; i++)
        {
            PlayerManager.Instance.PlayerDied(collider2Ds[i].gameObject.GetComponent<PlayerUnit>().PlayerId);
        }
        ArrowManager.Instance.DestroyArrow(this);
    }

    public override void Freeze()
    {
        if (HasHit) return; // so that stuck explosive arrows do not get affected 
        base.Freeze();
    }

    public override void UnFreeze()
    {
        if (HasHit) return; // so that stuck explosive arrows do not get affected 
        base.UnFreeze();
    }
}
