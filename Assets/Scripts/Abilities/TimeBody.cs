using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    public float timeBeforeAffected;  // Time after object spawns until  affected by Timestop
    private TimeManager timeManager;
    private Rigidbody2D rb;
    private Vector2 recordedVelocity;
    private float recordedMagnitude;

    private float TimeBeforeAffectedTimer;
    private bool canBeAffected;
    private bool IsStopped;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeManager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
        //TimeBeforeAffectedTimer = timeBeforeAffected;
        canBeAffected = true;

    }

    // Update is called once per frame
    void Update()
    {
        /*TimeBeforeAffectedTimer -= Time.deltaTime;
        if (TimeBeforeAffectedTimer <=0)
        {
            canBeAffected = true; // Will be affected by timestop
        }*/

        if (canBeAffected && timeManager.TimeIsStopped && !IsStopped )
        {
            if (rb.velocity.magnitude>=0f) // if it's  moving
            {
                recordedVelocity = rb.velocity.normalized; //records direction 
                recordedMagnitude = rb.velocity.magnitude; // records magnitude

                rb.velocity = Vector2.zero; // Stopes Moving rb
                rb.isKinematic = true; // not get affected by forces 
                IsStopped = true; 
            
            }
        }
    }


    public void ContinueTime()
    {
        rb.isKinematic = false;
        IsStopped = false;
        rb.velocity = recordedVelocity * recordedMagnitude;  // Adding back object's velocity
    }
}
