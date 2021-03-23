using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleChest : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayerMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayerMask | 1 << collision.gameObject.layer) == playerLayerMask)
        {

        }
    }
}
