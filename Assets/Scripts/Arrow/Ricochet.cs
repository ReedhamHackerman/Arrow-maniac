using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : Arrow
{
    public override void OnHit(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            
        }
    }
}
