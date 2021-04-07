using UnityEngine;

public class Explosive : Arrow
{

    private GameObject explosionIndication;
    [SerializeField] private AudioClip timerClock;
    [SerializeField] private AudioClip directHitBombExplosion;
    //Specify the time That Arrow Should Stuck in the Object
    [SerializeField] private float explodeAfterTimer;
    //Specify the Explosion Radius For The AOE Damage In That Area
    [SerializeField] private float explosionRadius;
    public override void OnHit(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        }
        Stuck();
    }

    private void Stuck()
    {
        
        audioSourceArrow.clip = timerClock;
        audioSourceArrow.loop = timerClock;
        audioSourceArrow.Play();
        HasHit = true;
        RB2D.velocity = Vector2.zero;
        RB2D.isKinematic = true;
        explosionIndication = Instantiate(ArrowManager.Instance.ExplosionRadiusIndication, transform.position, Quaternion.identity);
        TimeManager.Instance.AddDelegate(() => Explode(), explodeAfterTimer, 1);
    }

    private void Explode()
    {
        ExplosionParticleEffect();
        Destroy(explosionIndication);
        AudioSource.PlayClipAtPoint(directHitBombExplosion, GameManager.Instance.MainCamera.transform.position, 0.85f);
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, explosionRadius, LayerMask.GetMask("Player"));
        for (int i = 0; i < collider2Ds.Length; i++)
        {
            PlayerManager.Instance.PlayerDied(collider2Ds[i].gameObject.GetComponent<PlayerUnit>().PlayerId);
        }
        ArrowManager.Instance.DestroyArrow(this);
        
    }

    private void ExplosionParticleEffect()
    {
        ParticleSystem particleEffect = Instantiate(ArrowManager.Instance.ExplosionPartical, transform.position,Quaternion.identity);
        
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
