using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
public enum ArrowType
{
    RICOCHET,
    NORMAL,
    EXPLOSIVE
}
public class Arrow : MonoBehaviour
{
    public ArrowType arrowType;
    Rigidbody2D rb;

    private void ArrowRotation()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Update()
    {
        ArrowRotation();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Awake()
    {
        
    }
   
}
