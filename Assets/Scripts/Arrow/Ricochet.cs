using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochet : Arrow
{
    public override void OnHitWall(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {

        }
    }
}
