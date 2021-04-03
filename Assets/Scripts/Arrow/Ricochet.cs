using UnityEngine;

public class Ricochet : Arrow
{
    //Get The Current Velocity
    Vector3 lastVelocity;
    //Destroy After This Time 
    public float DestroyAfterTimer;
    //reduce by the factor of given velocity
    // public float speedFactor;
    [SerializeField] private AudioClip ricochetHitSound;
    public override void ArrowRotation()
    {
        //Meant To be Empty
    }

    //public void Awake()
    //{
    //    base.Oninitialize();
    //    RB2D.gravityScale = 0.0f;
    //    TimeManager.Instance.AddDelegate(() => DestroyRicochetArrow(), DestroyAfterTimer, 1);
    //}

    public void Update()
    {

        ArrowRotation();
        lastVelocity = RB2D.velocity;

    }
    public override void Oninitialize()
    {
        base.Oninitialize();
        RB2D.gravityScale = 0.0f;
        TimeManager.Instance.AddDelegate(() => DestroyRicochetArrow(), DestroyAfterTimer, 1);
    }
    public override void OnUpdate()
    {
        ArrowRotation();
        lastVelocity = RB2D.velocity;

    }
    public override void OnHit(Collision2D collision)
    {
        HasHit = true;
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.PlayerDied(collision.gameObject.GetComponent<PlayerUnit>().PlayerId);
            DestroyRicochetArrow();
        }
        else
        {
            AudioSource.PlayClipAtPoint(ricochetHitSound, transform.position, 0.444f);
            Vector2 LastContact;
            //float speed = lastVelocity.magnitude;
            if (collision.contacts.Length>1)
            {
               LastContact = collision.contacts[1].normal;
            }
            else
            {
            LastContact = collision.contacts[0].normal;
            }

            Vector3 direction = Vector3.Reflect(lastVelocity.normalized, LastContact);
            RB2D.velocity = direction  *  shootForce ;
            float angle = Mathf.Atan2(RB2D.velocity.y, RB2D.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    public override void DestroyArrow()
    {
        ArrowManager.Instance.DestroyArrow(this);
    }

    public void DestroyRicochetArrow()
    {
        DestroyArrow();
    }
}
