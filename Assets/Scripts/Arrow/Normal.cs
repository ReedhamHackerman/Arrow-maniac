using UnityEngine;

public class Normal : Arrow
{



    [SerializeField] private AudioClip arrowStuckSound;
    public override void OnHit(Collision2D collision)
    {
        base.OnHit(collision);
        if (collision.gameObject.CompareTag("Ground"))
        {

            Stuck();


        }

    }

    private void Stuck()
    {
        AudioSource.PlayClipAtPoint(arrowStuckSound, GameManager.Instance.MainCamera.transform.position, 0.04f);
        HasHit = true;
        RB2D.velocity = Vector3.zero;
        RB2D.isKinematic = true;
        IsPickable = true;
        selfCollider2D.isTrigger = true;
    }

    public override void Freeze()
    {
        if (HasHit) return; // so that stuck normal arrows do not get affected 
        base.Freeze();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsPickable)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerUnit player = collision.gameObject.GetComponent<PlayerUnit>();
                player.EquipArrow(arrowType, 1);

                selfCollider2D.isTrigger = false;
                DestroyArrow();
            }

        }
    }

    public override void UnFreeze()
    {
        if (HasHit) return; // so that stuck normal arrows do not get affected 
        base.UnFreeze();
    }



}
