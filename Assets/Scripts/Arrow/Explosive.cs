using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Arrow
{
    public override void OnHitWall(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {

        }
    }

}
