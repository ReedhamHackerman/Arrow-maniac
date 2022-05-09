using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : Arrow
{
    [SerializeField]private float rotationForce;
    [SerializeField]private AudioClip directHitBombExplosion;
    [SerializeField]private float explosionRadius;
    private  PlayerUnit enemyPlayer;
    public override void Oninitialize()
    {
        base.Oninitialize();
        RB2D.gravityScale = 0.0f;
        enemyPlayer = PlayerManager.Instance?.GetAnotherPlayer(playerUnit);
    }

    public override void OnHit(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("Ground"))
        {
            Explode();
        }
      
    }
    private void ExplosionParticleEffect()
    {
        ParticleSystem particleEffect = Instantiate(ArrowManager.Instance.ExplosionPartical, transform.position, Quaternion.identity);

    }
    private void Explode()
    {
        ExplosionParticleEffect();
        AudioSource.PlayClipAtPoint(directHitBombExplosion, GameManager.Instance.MainCamera.transform.position, 0.85f);
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, explosionRadius, LayerMask.GetMask("Player"));
        for (int i = 0; i < collider2Ds.Length; i++)
        {
            PlayerManager.Instance.PlayerDied(collider2Ds[i].gameObject.GetComponent<PlayerUnit>().PlayerId);
        }
        ArrowManager.Instance.DestroyArrow(this);

    }
    public override void OnFixedUpdate()
    {
        Vector2 direction = base.AnotherPlayerDirection(enemyPlayer);
        base.AddForceInDirection(direction) ;
        RotateTowardsEnemy(direction);
        
    }

    public override void IgnoreSelfCollision(Collider2D playerCollider)
    {
        Physics2D.IgnoreCollision(playerCollider, selfCollider2D, true);

        TimeManager.Instance.AddDelegate(() => Physics2D.IgnoreCollision(playerCollider, selfCollider2D, false), 2f, 1);
    }
    public void RotateTowardsEnemy(Vector2 direction)
    {
        Vector3 vectorToTarget = direction;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationForce);

    }
    public override void Freeze()
    {
        if (HasHit) return;
        base.Freeze();
    }

    public override void UnFreeze()
    {
        if (HasHit) return; 
        base.UnFreeze();
    }
}