﻿using UnityEngine;
public enum ArrowType
{
    NORMAL,
    EXPLOSIVE,
    RICOCHET
}
public class Arrow : MonoBehaviour, IFreezable
{
    public ArrowType arrowType;
    [HideInInspector] protected Rigidbody2D RB2D { get; set; }
    [HideInInspector] protected bool HasHit { get; set; }

    protected Vector2 arrowValocity;

    protected float arrowSpeed;

    protected bool isTimeStopped;

    [SerializeField] private float shootForce;

    protected bool IsPickable { get; set; }

    protected Collider2D selfCollider2D;

    public virtual void ArrowRotation()
    {
        if (HasHit == false && !isTimeStopped)
        {
            float angle = Mathf.Atan2(RB2D.velocity.y, RB2D.velocity.x) * Mathf.Rad2Deg;
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

            if (!IsPickable)
            {
                PlayerManager.Instance.PlayerDied(collision.gameObject.GetComponent<PlayerUnit>().PlayerId);
                //Destroying Game Object Without Any Particle Effects Later On That logic will be Changed
                DestroyArrow();
            }
        }
    }

    public virtual void Oninitialize()
    {
        RB2D = GetComponent<Rigidbody2D>();
        selfCollider2D = GetComponent<Collider2D>();
        IsPickable = false;
    }

    public virtual void OnUpdate()
    {
        if (HasHit == false)
        {
            ArrowRotation();
        }

    }

    public void AddForceInDirection(Vector2 dir)
    {
        RB2D.velocity = dir * shootForce;
    }

    public virtual void DestroyArrow()
    {
        ArrowManager.Instance.DestroyArrow(this);
    }

    public virtual void Freeze()
    {
        isTimeStopped = true;
        arrowValocity = RB2D.velocity;
        RB2D.velocity = Vector3.zero;
        RB2D.isKinematic = true;
    }

    public virtual void UnFreeze()
    {
        isTimeStopped = false;
        RB2D.velocity = arrowValocity;
        RB2D.isKinematic = false;
        isTimeStopped = false;
    }
}
